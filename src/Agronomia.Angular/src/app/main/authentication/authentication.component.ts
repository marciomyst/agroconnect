import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { AuthService, LoginRequest } from './auth.service';
import { CarouselSlide } from '../shared/carousel-card/carousel-card.component';
import { BiometricAuthService } from './biometric-auth.service';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  standalone: false,
  styleUrl: './authentication.component.css'
})
export class AuthenticationComponent implements OnInit {
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
  biometricError = '';
  isBiometricSubmitting = false;
  isBiometricSupported = false;
  hasBiometricProfile = false;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly authService: AuthService,
    private readonly biometricAuthService: BiometricAuthService,
    private readonly router: Router
  ) {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      enableBiometrics: [true]
    });
  }

  ngOnInit(): void {
    void this.loadBiometricCapabilities();
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
    this.biometricError = '';

    const { email, password, enableBiometrics } = this.loginForm.getRawValue() as {
      email: string;
      password: string;
      enableBiometrics?: boolean;
    };

    const payload: LoginRequest = { email, password };
    const wantsBiometricEnrollment = !!enableBiometrics && this.isBiometricSupported;

    try {
      const loginResponse = await firstValueFrom(this.authService.login(payload));

      if (wantsBiometricEnrollment && loginResponse?.refreshToken) {
        await this.enableBiometricLogin(email, loginResponse.refreshToken);
      }

      await this.router.navigate(['/dashboard']);
    } catch (error) {
      this.errorMessage = this.getErrorMessage(error);
    } finally {
      this.isSubmitting = false;
    }
  }

  async onBiometricLogin(): Promise<void> {
    this.biometricError = '';

    if (!this.isBiometricSupported) {
      this.biometricError = 'Biometria não disponível neste dispositivo.';
      return;
    }

    if (!this.hasBiometricProfile) {
      this.biometricError = 'Ative o acesso por biometria após um login com senha.';
      return;
    }

    this.isBiometricSubmitting = true;

    try {
      const profile = await this.biometricAuthService.authenticate();
      const response = await firstValueFrom(this.authService.refreshSession(profile.refreshToken));
      this.biometricAuthService.updateRefreshToken(response.refreshToken);
      await this.router.navigate(['/dashboard']);
    } catch (error) {
      this.biometricError = this.getBiometricErrorMessage(error);
    } finally {
      this.isBiometricSubmitting = false;
    }
  }

  private async loadBiometricCapabilities(): Promise<void> {
    this.isBiometricSupported = await this.biometricAuthService.isSupported();
    this.hasBiometricProfile = this.biometricAuthService.hasEnrollment();
  }

  private async enableBiometricLogin(email: string, refreshToken: string): Promise<void> {
    try {
      const profile = await this.biometricAuthService.enroll(email, refreshToken);
      this.hasBiometricProfile = !!profile;
    } catch (error) {
      this.biometricError = this.getBiometricErrorMessage(error);
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

  private getBiometricErrorMessage(error: unknown): string {
    if (error instanceof HttpErrorResponse) {
      if (error.status === 0) {
        return 'Não foi possível conectar ao servidor para validar a biometria.';
      }

      if (error.status === 401) {
        return 'Sessão biométrica expirada. Faça login com senha e reative a biometria.';
      }

      if (typeof error.error === 'string' && error.error.trim().length > 0) {
        return error.error;
      }
    }

    if (error instanceof DOMException) {
      if (error.name === 'NotAllowedError') {
        return 'Autenticação biométrica cancelada ou não autorizada.';
      }

      if (error.name === 'InvalidStateError') {
        return 'A biometria já está registrada ou não pode ser usada agora.';
      }
    }

    if (error instanceof Error && error.message) {
      return error.message;
    }

    return 'Não foi possível concluir a autenticação biométrica.';
  }
}
