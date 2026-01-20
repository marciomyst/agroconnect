import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize, map, switchMap } from 'rxjs';
import { AuthApiService } from '../../core/auth/auth-api.service';
import { AuthTokenService } from '../../core/auth/auth-token.service';
import { CurrentUserStore } from '../../core/auth/current-user.store';
import { ActiveOrganizationStore } from '../../core/organization/active-organization.store';

@Component({
  selector: 'app-public-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <section class="public-login">
      <h1>Login</h1>
      <form [formGroup]="form" (ngSubmit)="submit()">
        <label>
          <span>Email</span>
          <input type="email" formControlName="email" autocomplete="email" />
        </label>
        <label>
          <span>Password</span>
          <input type="password" formControlName="password" autocomplete="current-password" />
        </label>
        <button type="submit" [disabled]="form.invalid || submitting()">Sign in</button>
        <p class="error" *ngIf="errorMessage()">{{ errorMessage() }}</p>
      </form>
    </section>
  `,
})
export class PublicLoginComponent {
  private readonly authApi = inject(AuthApiService);
  private readonly tokenService = inject(AuthTokenService);
  private readonly currentUserStore = inject(CurrentUserStore);
  private readonly activeOrganizationStore = inject(ActiveOrganizationStore);
  private readonly router = inject(Router);

  readonly submitting = signal(false);
  readonly errorMessage = signal<string | null>(null);

  readonly form = new FormGroup({
    email: new FormControl('', { nonNullable: true, validators: [Validators.required, Validators.email] }),
    password: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
  });

  submit(): void {
    if (this.submitting()) {
      return;
    }

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting.set(true);
    this.errorMessage.set(null);

    const payload = this.form.getRawValue();

    this.authApi.login(payload).pipe(
      map(response => {
        const token = response.accessToken ?? response.token;
        if (!token) {
          throw new Error('Token missing in response');
        }
        this.tokenService.setToken(token);
        return token;
      }),
      switchMap(() => this.currentUserStore.loadFromApi()),
      finalize(() => this.submitting.set(false))
    ).subscribe({
      next: context => {
        if (!context) {
          this.errorMessage.set('Unable to load user context.');
          return;
        }
        this.activeOrganizationStore.loadFromStorage();
        if (!this.activeOrganizationStore.hasActiveOrganization()) {
          void this.router.navigateByUrl('/select-organization');
          return;
        }
        void this.router.navigateByUrl('/');
      },
      error: () => {
        this.errorMessage.set('Invalid credentials.');
      },
    });
  }
}
