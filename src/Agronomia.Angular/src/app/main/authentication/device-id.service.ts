import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DeviceIdService {
  private readonly storageKey = 'agroconnect.deviceId';

  getOrCreate(): string {
    const existing = localStorage.getItem(this.storageKey);
    if (existing) {
      return existing;
    }

    const generated = self.crypto ? this.generateRandomId() : this.fallbackId();
    localStorage.setItem(this.storageKey, generated);
    return generated;
  }

  private generateRandomId(): string {
    const bytes = new Uint8Array(16);
    crypto.getRandomValues(bytes);
    return Array.from(bytes, b => b.toString(16).padStart(2, '0')).join('');
  }

  private fallbackId(): string {
    return `${Date.now()}-${Math.random().toString(16).slice(2)}`;
  }
}
