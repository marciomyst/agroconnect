import { Component } from '@angular/core';

@Component({
  selector: 'app-password-reset',
  templateUrl: './password-reset.component.html',
  standalone: false,
  styleUrl: './password-reset.component.css'
})
export class PasswordResetComponent {
  showSuccessModal = false;

  openSuccessModal(): void {
    this.showSuccessModal = true;
  }

  closeSuccessModal(): void {
    this.showSuccessModal = false;
  }
}
