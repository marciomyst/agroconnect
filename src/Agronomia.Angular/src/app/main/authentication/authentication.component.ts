import { Component } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { AuthService, LoginRequest } from './auth.service';
import { CarouselSlide } from '../shared/carousel-card/carousel-card.component';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  standalone: false,
  styleUrl: './authentication.component.css'
})
export class AuthenticationComponent {
  readonly loginForm: FormGroup;
  readonly carouselSlides: CarouselSlide[] = [
    {
      type: 'testimonial',
      rating: 5,
      quote:
        'O AgroConnect transformou a maneira como negociamos insumos. Encontramos os melhores precos e agilidade na entrega com apenas alguns cliques.',
      author: 'Carlos Mendes',
      authorRole: 'Produtor Rural - Mato Grosso',
      authorImage:
        'https://lh3.googleusercontent.com/aida-public/AB6AXuBSVXBCwAbfrPXoL8VvaaQJ_G46AB-qCWN9Ib-A1AAt4LZEGZb6zHx2bZX4k7QKapIN9CovXUTOxJF8TesfMMEBIqMJbGsLn-KeO-6lDTW0F6tWh7xIgBryRAIsG_PeIyDRiGzHtjpPxDi_j7r3dVt8sB_1ocBQqGlnq9j5xcbaGV73wf-QTSu6SnevDUVgjJwfVMiqBytLu8XSV3NlqpAZEpe7I1fN4zxMgv1Vo3UhCPoyvzQLZR272IYgCnzKsyfkSCNHWOZVWmUa'
    }
  ];

  isSubmitting = false;
  passwordVisible = false;
  errorMessage = '';

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly authService: AuthService,
    private readonly router: Router
  ) {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  get emailControl() {
    return this.loginForm.get('email');
  }

  get passwordControl() {
    return this.loginForm.get('password');
  }

  get passwordFieldType(): 'password' | 'text' {
    return this.passwordVisible ? 'text' : 'password';
  }

  togglePasswordVisibility(): void {
    this.passwordVisible = !this.passwordVisible;
  }

  async onSubmit(): Promise<void> {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    const payload = this.loginForm.getRawValue() as LoginRequest;

    try {
      await firstValueFrom(this.authService.login(payload));
      await this.router.navigate(['/dashboard']);
    } catch (error) {
      this.errorMessage = this.getErrorMessage(error);
    } finally {
      this.isSubmitting = false;
    }
  }

  private getErrorMessage(error: unknown): string {
    if (error instanceof HttpErrorResponse) {
      if (error.status === 0) {
        return 'Nao foi possivel conectar ao servidor. Verifique sua conexao.';
      }

      if (error.status === 401) {
        return 'Credenciais invalidas. Verifique seus dados e tente novamente.';
      }

      if (typeof error.error === 'string' && error.error.trim().length > 0) {
        return error.error;
      }

      if (error.error?.title) {
        return error.error.title;
      }
    }

    return 'Ocorreu um erro ao tentar autenticar. Tente novamente em instantes.';
  }
}
