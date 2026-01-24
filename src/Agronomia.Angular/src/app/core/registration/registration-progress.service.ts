import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class RegistrationProgressService {
  private readonly pendingFarmKey = 'agroconnect.registration.pending-farm';
  private readonly pendingSellerKey = 'agroconnect.registration.pending-seller';

  setPendingFarmRegistration(): void {
    localStorage.setItem(this.pendingFarmKey, '1');
  }

  clearPendingFarmRegistration(): void {
    localStorage.removeItem(this.pendingFarmKey);
  }

  hasPendingFarmRegistration(): boolean {
    return localStorage.getItem(this.pendingFarmKey) === '1';
  }

  setPendingSellerRegistration(): void {
    localStorage.setItem(this.pendingSellerKey, '1');
  }

  clearPendingSellerRegistration(): void {
    localStorage.removeItem(this.pendingSellerKey);
  }

  hasPendingSellerRegistration(): boolean {
    return localStorage.getItem(this.pendingSellerKey) === '1';
  }
}
