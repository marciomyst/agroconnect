import { Component, inject } from '@angular/core';
import { AbstractControl, FormBuilder, Validators, ValidationErrors } from '@angular/forms';
import { Router } from '@angular/router';
import { SellerApiService, CreateSellerPayload } from '../../seller/seller.api.service';

@Component({
  selector: 'app-seller-onboarding',
  templateUrl: './seller-onboarding.component.html',
  standalone: false,
  styleUrl: './seller-onboarding.component.css'
})
export class SellerOnboardingComponent {
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly sellerApi = inject(SellerApiService);

  currentStep = 1;
  readonly totalSteps = 3;
  readonly steps = [
    { id: 1, label: 'Dados da Empresa', icon: 'domain' },
    { id: 2, label: 'Endereço', icon: 'location_on' },
    { id: 3, label: 'Responsável e Acesso', icon: 'person' }
  ];

  readonly form = this.fb.group(
    {
      company: this.fb.group({
        cnpj: ['', [Validators.required]],
        stateRegistration: ['', [Validators.required]],
        legalName: ['', [Validators.required]],
        tradeName: ['', [Validators.required]]
      }),
      address: this.fb.group({
        zipCode: ['', [Validators.required]],
        street: ['', [Validators.required]],
        number: ['', [Validators.required]],
        complement: [''],
        city: ['', [Validators.required]],
        state: ['', [Validators.required]]
      }),
      access: this.fb.group({
        responsibleName: ['', [Validators.required]],
        phone: ['', [Validators.required]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(8)]],
        confirmPassword: ['', [Validators.required]],
        terms: [false, [Validators.requiredTrue]]
      })
    },
    { validators: SellerOnboardingComponent.passwordsMatchValidator }
  );

  static passwordsMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('access.password')?.value;
    const confirm = control.get('access.confirmPassword')?.value;
    if (password && confirm && password !== confirm) {
      return { passwordMismatch: true };
    }
    return null;
  }

  get companyGroup() {
    return this.form.get('company');
  }

  get addressGroup() {
    return this.form.get('address');
  }

  get accessGroup() {
    return this.form.get('access');
  }

  isSubmitting = false;
  submitError = '';

  get isLastStep(): boolean {
    return this.currentStep === this.totalSteps;
  }

  getControl(path: string) {
    return this.form.get(path);
  }

  goToNextStep(): void {
    if (this.isCurrentStepValid() && this.currentStep < this.totalSteps) {
      this.currentStep += 1;
      return;
    }
    this.touchCurrentStepControls();
  }

  goToPreviousStep(): void {
    if (this.currentStep > 1) {
      this.currentStep -= 1;
    }
  }

  onSubmit(): void {
    if (!this.form.valid) {
      this.touchAll();
      return;
    }

    const payload = this.buildPayload();
    this.isSubmitting = true;
    this.submitError = '';

    this.sellerApi.createSeller(payload).subscribe({
      next: () => {
        this.isSubmitting = false;
        void this.router.navigate(['/marketplace']);
      },
      error: () => {
        this.isSubmitting = false;
        this.submitError = 'Não foi possível salvar os dados. Tente novamente.';
      }
    });
  }

  private isCurrentStepValid(): boolean {
    const group = this.getGroupByStep(this.currentStep);
    return !!group && group.valid;
  }

  private touchCurrentStepControls(): void {
    const group = this.getGroupByStep(this.currentStep);
    group?.markAllAsTouched();
  }

  private touchAll(): void {
    this.form.markAllAsTouched();
  }

  private getGroupByStep(step: number) {
    switch (step) {
      case 1:
        return this.companyGroup;
      case 2:
        return this.addressGroup;
      case 3:
        return this.accessGroup;
      default:
        return null;
    }
  }

  private buildPayload(): CreateSellerPayload {
    const value = this.form.getRawValue();
    return {
      cnpj: value.company?.cnpj ?? '',
      stateRegistration: value.company?.stateRegistration ?? '',
      legalName: value.company?.legalName ?? '',
      tradeName: value.company?.tradeName ?? '',
      contactEmail: value.access?.email ?? '',
      contactPhone: value.access?.phone ?? '',
      responsibleName: value.access?.responsibleName ?? '',
      zipCode: value.address?.zipCode ?? '',
      street: value.address?.street ?? '',
      number: value.address?.number ?? '',
      city: value.address?.city ?? '',
      state: value.address?.state ?? '',
      complement: value.address?.complement ?? '',
      password: value.access?.password ?? ''
    };
  }
}
