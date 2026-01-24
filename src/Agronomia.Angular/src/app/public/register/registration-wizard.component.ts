import { CommonModule } from '@angular/common';
import { Component, DestroyRef, inject, signal } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { catchError, finalize, map, of, switchMap, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AuthApiService } from '../../core/auth/auth-api.service';
import { AuthTokenService } from '../../core/auth/auth-token.service';
import { CurrentUserStore } from '../../core/auth/current-user.store';
import { ActiveOrganizationStore } from '../../core/organization/active-organization.store';
import { RegistrationProgressService } from '../../core/registration/registration-progress.service';
import { RegistrationApiService } from './registration-api.service';

const passwordMatchValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
  const group = control as { get: (name: string) => AbstractControl | null };
  const password = group.get('password')?.value;
  const confirmPassword = group.get('confirmPassword')?.value;

  if (!password || !confirmPassword) {
    return null;
  }

  return password === confirmPassword ? null : { passwordMismatch: true };
};

type WizardStep = 'user' | 'farm';

@Component({
  selector: 'app-registration-wizard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './registration-wizard.component.html',
  styleUrls: ['./registration-wizard.component.scss'],
})
export class RegistrationWizardComponent {
  private readonly authApi = inject(AuthApiService);
  private readonly tokenService = inject(AuthTokenService);
  private readonly currentUserStore = inject(CurrentUserStore);
  private readonly activeOrganizationStore = inject(ActiveOrganizationStore);
  private readonly registrationApi = inject(RegistrationApiService);
  private readonly registrationProgress = inject(RegistrationProgressService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly formBuilder = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);
  private pendingCredentials: { email: string; password: string } | null = null;

  readonly step = signal<WizardStep>('user');
  readonly resumeRequired = signal(false);
  readonly isAccountStepSkipped = signal(false);

  readonly isSubmittingUser = signal(false);
  readonly isSubmittingFarm = signal(false);
  readonly userErrorMessage = signal<string | null>(null);
  readonly farmErrorMessage = signal<string | null>(null);

  readonly userForm = this.formBuilder.nonNullable.group(
    {
      name: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required]],
    },
    { validators: [passwordMatchValidator] }
  );

  readonly farmForm = this.formBuilder.nonNullable.group({
    taxId: ['', [Validators.required]],
    name: ['', [Validators.required]],
  });

  constructor() {
    const requestedStep = this.route.snapshot.queryParamMap.get('step');
    const hasToken = this.tokenService.hasToken();
    const wantsFarmStep = requestedStep === 'farm'
      || this.registrationProgress.hasPendingFarmRegistration();

    if (hasToken) {
      this.isAccountStepSkipped.set(true);
      this.step.set('farm');
    } else if (wantsFarmStep) {
      this.resumeRequired.set(true);
    }

    if (hasToken && !this.currentUserStore.isLoaded()) {
      this.currentUserStore.loadFromApi().pipe(
        takeUntilDestroyed(this.destroyRef)
      ).subscribe();
    }
  }

  submitUser(): void {
    if (this.isSubmittingUser()) {
      return;
    }

    if (this.userForm.invalid) {
      this.userForm.markAllAsTouched();
      return;
    }

    this.isSubmittingUser.set(true);
    this.userErrorMessage.set(null);
    this.clearServerErrors(this.userForm);

    const { name, email, password } = this.userForm.getRawValue();
    const payload = {
      name: name.trim(),
      email: email.trim(),
      password,
    };

    this.registrationApi.registerUser(payload).pipe(
      switchMap(() => this.authApi.login({ email: payload.email, password })),
      tap(response => {
        const token = response.accessToken ?? response.token;
        if (!token) {
          throw new Error('Token missing in response');
        }
        this.tokenService.setToken(token);
      }),
      switchMap(() => this.currentUserStore.loadFromApi()),
      finalize(() => this.isSubmittingUser.set(false))
    ).subscribe({
      next: context => {
        if (!context) {
          this.userErrorMessage.set('Nao foi possivel carregar seus dados. Tente novamente.');
          return;
        }

        this.pendingCredentials = { email: payload.email, password };
        this.registrationProgress.setPendingFarmRegistration();
        this.goToFarmStep();
      },
      error: error => {
        const mapped = this.applyFieldErrors(this.userForm, {
          Name: 'name',
          Email: 'email',
          Password: 'password',
        }, error);

        if (!mapped) {
          this.userErrorMessage.set(this.resolveUserErrorMessage(error));
        }
      },
    });
  }

  submitFarm(): void {
    if (this.isSubmittingFarm()) {
      return;
    }

    if (!this.tokenService.hasToken()) {
      this.farmErrorMessage.set('Faca login para continuar o cadastro da Farm.');
      return;
    }

    if (this.farmForm.invalid) {
      this.farmForm.markAllAsTouched();
      return;
    }

    this.isSubmittingFarm.set(true);
    this.farmErrorMessage.set(null);
    this.clearServerErrors(this.farmForm);

    const { taxId, name } = this.farmForm.getRawValue();
    const payload = {
      taxId: taxId.trim(),
      name: name.trim(),
    };

    this.registrationApi.registerFarm(payload).pipe(
      switchMap(response =>
        this.authenticateAfterFarmRegistration().pipe(
          map(() => response)
        )
      ),
      switchMap(response =>
        this.currentUserStore.loadFromApi().pipe(
          map(context => ({ context, response }))
        )
      ),
      finalize(() => this.isSubmittingFarm.set(false))
    ).subscribe({
      next: ({ context, response }) => {
        this.registrationProgress.clearPendingFarmRegistration();
        this.pendingCredentials = null;

        if (!context) {
          this.farmErrorMessage.set('Cadastro concluido, mas nao foi possivel carregar seus dados.');
          return;
        }

        const farm = context.organizations.find(organization =>
          organization.organizationId === response.farmId && organization.type === 'Farm'
        );

        if (farm) {
          this.activeOrganizationStore.setActiveOrganization({
            organizationId: farm.organizationId,
            organizationName: farm.organizationName,
            organizationType: farm.type,
            roles: farm.roles,
          });
        }

        void this.router.navigate(['/farmer/marketplace']);
      },
      error: error => {
        const mapped = this.applyFieldErrors(this.farmForm, {
          TaxId: 'taxId',
          Name: 'name',
        }, error);

        if (!mapped) {
          this.farmErrorMessage.set(this.resolveFarmErrorMessage(error));
        }
      },
    });
  }

  cancelFarmRegistration(): void {
    this.pendingCredentials = null;
    this.tokenService.clearToken();
    this.currentUserStore.clear();
    this.activeOrganizationStore.clear();
    void this.router.navigate(['/login']);
  }

  private goToFarmStep(): void {
    this.step.set('farm');
    this.resumeRequired.set(false);
    void this.router.navigate([], {
      queryParams: { step: 'farm' },
      queryParamsHandling: 'merge',
      replaceUrl: true,
    });
  }

  private resolveUserErrorMessage(error: unknown): string {
    const status = this.getErrorStatus(error);
    if (status === 409) {
      return 'Este e-mail ja esta cadastrado.';
    }
    if (status === 0 || (status !== null && status >= 500)) {
      return 'Nao foi possivel criar sua conta. Tente novamente.';
    }
    return 'Nao foi possivel criar sua conta. Verifique os dados e tente novamente.';
  }

  private authenticateAfterFarmRegistration() {
    if (!this.pendingCredentials) {
      return of(null);
    }

    return this.authApi.login(this.pendingCredentials).pipe(
      tap(response => {
        const token = response.accessToken ?? response.token;
        if (!token) {
          throw new Error('Token missing in response');
        }
        this.tokenService.setToken(token);
      }),
      map(() => null),
      catchError(() => of(null))
    );
  }

  private resolveFarmErrorMessage(error: unknown): string {
    const status = this.getErrorStatus(error);
    if (status === 409) {
      return 'Ja existe uma Farm com este TaxId.';
    }
    if (status === 0 || (status !== null && status >= 500)) {
      return 'Nao foi possivel cadastrar a Farm. Tente novamente.';
    }
    return 'Nao foi possivel cadastrar a Farm. Verifique os dados e tente novamente.';
  }

  private getErrorStatus(error: unknown): number | null {
    if (!error || typeof error !== 'object') {
      return null;
    }

    const status = (error as Record<string, unknown>)['status'];
    return typeof status === 'number' ? status : null;
  }

  private applyFieldErrors(
    form: { controls: Record<string, AbstractControl> },
    mapping: Record<string, string>,
    error: unknown
  ): boolean {
    const fieldErrors = this.getFieldErrors(error);
    if (!fieldErrors) {
      return false;
    }

    for (const [field, messages] of Object.entries(fieldErrors)) {
      const controlName = mapping[field];
      if (!controlName) {
        continue;
      }

      const control = form.controls[controlName];
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

    return true;
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

  private clearServerErrors(form: { controls: Record<string, AbstractControl> }): void {
    for (const control of Object.values(form.controls)) {
      if (control.errors?.['server']) {
        const remaining = { ...control.errors };
        delete remaining['server'];
        control.setErrors(Object.keys(remaining).length ? remaining : null);
      }
    }
  }
}
