import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { AuthApiService } from '../core/auth/auth-api.service';

const passwordMatchValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
  const group = control as { get: (name: string) => AbstractControl | null };
  const newPassword = group.get('newPassword')?.value;
  const confirmPassword = group.get('confirmPassword')?.value;

  if (!newPassword || !confirmPassword) {
    return null;
  }

  return newPassword === confirmPassword ? null : { passwordMismatch: true };
};

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss'],
})
export class ChangePasswordComponent {
  private readonly authApi = inject(AuthApiService);
  private readonly formBuilder = inject(FormBuilder);

  readonly isLoading = signal(false);
  readonly successMessage = signal<string | null>(null);
  readonly errorMessage = signal<string | null>(null);

  readonly form = this.formBuilder.nonNullable.group(
    {
      currentPassword: ['', [Validators.required]],
      newPassword: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required]],
    },
    { validators: [passwordMatchValidator] }
  );

  onSubmit(): void {
    if (this.isLoading()) {
      return;
    }

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);
    this.successMessage.set(null);
    this.errorMessage.set(null);

    const { currentPassword, newPassword } = this.form.getRawValue();

    this.authApi.changePassword({ currentPassword, newPassword }).pipe(
      finalize(() => this.isLoading.set(false))
    ).subscribe({
      next: () => {
        this.successMessage.set('Your password has been updated successfully.');
        this.form.reset({
          currentPassword: '',
          newPassword: '',
          confirmPassword: '',
        });
      },
      error: (error: unknown) => {
        this.errorMessage.set(this.resolveErrorMessage(error));
      },
    });
  }

  private resolveErrorMessage(error: unknown): string {
    if (this.isCurrentPasswordInvalid(error)) {
      return 'Current password is incorrect.';
    }

    const status = this.getErrorStatus(error);
    if (status === 0 || (status !== null && status >= 500)) {
      return 'We couldn\'t update your password. Please try again in a moment.';
    }

    const message = this.getErrorMessage(error);
    if (message) {
      return message;
    }

    return 'We couldn\'t update your password. Please try again in a moment.';
  }

  private isCurrentPasswordInvalid(error: unknown): boolean {
    const code = this.getErrorCode(error);
    if (code && code.toLowerCase().includes('current')) {
      return true;
    }

    const fieldErrors = this.getFieldErrors(error);
    if (fieldErrors?.['currentPassword']?.length) {
      return true;
    }

    const message = this.getErrorMessage(error);
    return !!message && message.toLowerCase().includes('current password');
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

  private getErrorCode(error: unknown): string | null {
    if (!error || typeof error !== 'object') {
      return null;
    }

    const errorRecord = error as Record<string, unknown>;
    const directCode = errorRecord['code'];
    if (typeof directCode === 'string' && directCode.length > 0) {
      return directCode;
    }

    const nested = errorRecord['error'];
    if (nested && typeof nested === 'object') {
      const nestedCode = (nested as Record<string, unknown>)['code'];
      if (typeof nestedCode === 'string' && nestedCode.length > 0) {
        return nestedCode;
      }
    }

    return null;
  }

  private getFieldErrors(error: unknown): Record<string, string[]> | null {
    if (!error || typeof error !== 'object') {
      return null;
    }

    const errorRecord = error as Record<string, unknown>;
    const directFieldErrors = errorRecord['fieldErrors'];
    if (directFieldErrors && typeof directFieldErrors === 'object' && !Array.isArray(directFieldErrors)) {
      return directFieldErrors as Record<string, string[]>;
    }

    const nested = errorRecord['error'];
    if (nested && typeof nested === 'object') {
      const nestedFieldErrors = (nested as Record<string, unknown>)['fieldErrors'];
      if (nestedFieldErrors && typeof nestedFieldErrors === 'object' && !Array.isArray(nestedFieldErrors)) {
        return nestedFieldErrors as Record<string, string[]>;
      }
    }

    return null;
  }
}
