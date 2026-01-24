import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { finalize, switchMap, tap } from 'rxjs';
import { AuthApiService } from '../../core/auth/auth-api.service';
import { AuthTokenService } from '../../core/auth/auth-token.service';
import { CurrentUserContext } from '../../core/auth/auth.types';
import { CurrentUserStore } from '../../core/auth/current-user.store';
import { ActiveOrganizationStore } from '../../core/organization/active-organization.store';
import { RegistrationProgressService } from '../../core/registration/registration-progress.service';

@Component({
  selector: 'app-public-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class PublicLoginComponent {
  private readonly authApi = inject(AuthApiService);
  private readonly tokenService = inject(AuthTokenService);
  private readonly currentUserStore = inject(CurrentUserStore);
  private readonly activeOrganizationStore = inject(ActiveOrganizationStore);
  private readonly registrationProgress = inject(RegistrationProgressService);
  private readonly router = inject(Router);
  private readonly formBuilder = inject(FormBuilder);

  readonly isLoading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly isPasswordVisible = signal(false);
  readonly isSignupSelectionVisible = signal(false);

  readonly form = this.formBuilder.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
  });

  togglePasswordVisibility(): void {
    this.isPasswordVisible.update(value => !value);
  }

  showSignupSelection(): void {
    this.isSignupSelectionVisible.set(true);
  }

  showLogin(): void {
    this.isSignupSelectionVisible.set(false);
  }

  onSubmit(): void {
    if (this.isLoading()) {
      return;
    }

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.clearFieldErrors();

    const { email, password } = this.form.getRawValue();

    const payload = { email, password };

    this.authApi.login(payload).pipe(
      tap(response => {
        const token = response.accessToken ?? response.token;
        if (!token) {
          throw new Error('Token missing in response');
        }
        this.tokenService.setToken(token);
      }),
      switchMap(() => this.currentUserStore.loadFromApi()),
      finalize(() => this.isLoading.set(false))
    ).subscribe({
      next: context => {
        if (!context) {
          this.errorMessage.set('Nao foi possivel carregar seus dados. Tente novamente.');
          return;
        }
        this.activeOrganizationStore.loadFromStorage();
        this.ensureSingleOrganizationSelected(context);
        // Navigation depends on whether an active organization is already resolved.
        if (!this.activeOrganizationStore.hasActiveOrganization()) {
          if (context.organizations.length === 0) {
            if (this.registrationProgress.hasPendingSellerRegistration()) {
              void this.router.navigate(['/register/seller'], { queryParams: { step: 'seller' } });
              return;
            }
            if (this.registrationProgress.hasPendingFarmRegistration()) {
              void this.router.navigate(['/register/farmer'], { queryParams: { step: 'farm' } });
              return;
            }
          }
          void this.router.navigate(['/select-organization']);
          return;
        }
        void this.router.navigate(['/']);
      },
      error: (error: unknown) => {
        const fieldErrors = this.getFieldErrors(error);
        this.applyFieldErrors(fieldErrors);
        if (!fieldErrors) {
          this.errorMessage.set(this.resolveErrorMessage(error));
        }
        this.form.controls.password.setValue('');
      },
    });
  }

  private ensureSingleOrganizationSelected(context: CurrentUserContext): void {
    if (this.activeOrganizationStore.hasActiveOrganization()) {
      return;
    }

    if (context.organizations.length !== 1) {
      return;
    }

    const organization = context.organizations[0];
    this.activeOrganizationStore.setActiveOrganization({
      organizationId: organization.organizationId,
      organizationName: organization.organizationName,
      organizationType: organization.type,
      roles: organization.roles,
    });
  }

  private resolveErrorMessage(error: unknown): string {
    const status = this.getErrorStatus(error);
    if (status === 401) {
      return 'Usuario ou senha invalidos.';
    }

    if (status === 0 || (status !== null && status >= 500)) {
      return 'Não foi possivel entrar. Tente novamente em instantes.';
    }

    if (status !== null) {
      const message = this.getErrorMessage(error);
      if (message) {
        return message;
      }
    }

    return 'Não foi possivel entrar. Tente novamente em instantes.';
  }

  private getErrorStatus(error: unknown): number | null {
    if (!error || typeof error !== 'object') {
      return null;
    }

    const status = (error as Record<string, unknown>)['status'];
    return typeof status === 'number' ? status : null;
  }

  private getErrorMessage(error: unknown): string | null {
    if (!error || typeof error !== 'object') {
      return null;
    }

    const errorRecord = error as Record<string, unknown>;
    const nested = errorRecord['error'];
    if (nested && typeof nested === 'object') {
      const nestedMessage = (nested as Record<string, unknown>)['message'];
      if (typeof nestedMessage === 'string' && nestedMessage.length > 0) {
        return nestedMessage;
      }
    }

    const directMessage = errorRecord['message'];
    if (typeof directMessage === 'string' && directMessage.length > 0) {
      return directMessage;
    }

    return null;
  }

  private getFieldErrors(error: unknown): Record<string, string[]> | null {
    if (!error || typeof error !== 'object') {
      return null;
    }

    const errorRecord = error as Record<string, unknown>;
    const fieldErrors = errorRecord['errors'] ?? errorRecord['fieldErrors'];
    if (fieldErrors && typeof fieldErrors === 'object' && !Array.isArray(fieldErrors)) {
      return fieldErrors as Record<string, string[]>;
    }

    const nested = errorRecord['error'];
    if (nested && typeof nested === 'object') {
      const nestedFieldErrors = (nested as Record<string, unknown>)['errors']
        ?? (nested as Record<string, unknown>)['fieldErrors'];
      if (nestedFieldErrors && typeof nestedFieldErrors === 'object' && !Array.isArray(nestedFieldErrors)) {
        return nestedFieldErrors as Record<string, string[]>;
      }
    }

    return null;
  }

  private applyFieldErrors(fieldErrors: Record<string, string[]> | null): void {
    if (!fieldErrors) {
      return;
    }

    const mapping: Record<string, keyof typeof this.form.controls> = {
      Email: 'email',
      Password: 'password',
    };

    for (const [field, messages] of Object.entries(fieldErrors)) {
      const controlName = mapping[field];
      if (!controlName) {
        continue;
      }

      const control = this.form.controls[controlName];
      if (!control) {
        continue;
      }

      const message = messages.join(' ');
      const existingErrors = control.errors ?? {};
      control.setErrors({
        ...existingErrors,
        server: message,
      });
      control.markAsTouched();
    }
  }

  private clearFieldErrors(): void {
    for (const control of Object.values(this.form.controls)) {
      if (control.errors?.['server']) {
        const remaining = { ...control.errors };
        delete remaining['server'];
        control.setErrors(Object.keys(remaining).length ? remaining : null);
      }
    }
  }
}
