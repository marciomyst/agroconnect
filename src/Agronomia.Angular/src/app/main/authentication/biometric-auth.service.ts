import { Injectable } from '@angular/core';

type StoredBiometricProfile = {
  email: string;
  credentialId: string;
  userHandle: string;
  refreshToken: string;
  createdAt: string;
  rpId?: string;
};

type ClientDataJSON = {
  type: string;
  challenge: string;
  origin: string;
  crossOrigin?: boolean;
};

@Injectable({
  providedIn: 'root'
})
export class BiometricAuthService {
  private readonly storageKey = 'agroconnect.biometric.profile';

  async isSupported(): Promise<boolean> {
    if (typeof window === 'undefined' || !('PublicKeyCredential' in window)) {
      return false;
    }

    try {
      return await PublicKeyCredential.isUserVerifyingPlatformAuthenticatorAvailable();
    } catch {
      return false;
    }
  }

  hasEnrollment(): boolean {
    return this.getStoredProfile() !== null;
  }

  getStoredProfile(): StoredBiometricProfile | null {
    const stored = localStorage.getItem(this.storageKey);
    if (!stored) {
      return null;
    }

    try {
      return JSON.parse(stored) as StoredBiometricProfile;
    } catch {
      return null;
    }
  }

  clearEnrollment(): void {
    localStorage.removeItem(this.storageKey);
  }

  async enroll(email: string, refreshToken: string): Promise<StoredBiometricProfile> {
    const supported = await this.isSupported();
    if (!supported) {
      throw new Error('Biometria não suportada neste dispositivo.');
    }

    const challenge = this.randomBuffer(32);
    const userId = await this.getUserId(email);

    const publicKey: PublicKeyCredentialCreationOptions = {
      challenge,
      rp: {
        id: window.location.hostname,
        name: 'AgroConnect'
      },
      user: {
        id: userId,
        name: email,
        displayName: email
      },
      pubKeyCredParams: [
        { type: 'public-key', alg: -7 },
        { type: 'public-key', alg: -257 }
      ],
      timeout: 60000,
      attestation: 'none',
      authenticatorSelection: {
        authenticatorAttachment: 'platform',
        userVerification: 'required',
        residentKey: 'preferred'
      },
      extensions: {
        credProps: true
      }
    };

    const credential = await navigator.credentials.create({ publicKey });
    if (!(credential instanceof PublicKeyCredential)) {
      throw new Error('Não foi possível registrar a credencial biométrica.');
    }

    const profile: StoredBiometricProfile = {
      email,
      credentialId: this.bufferToBase64Url(credential.rawId),
      userHandle: this.bufferToBase64Url(userId),
      refreshToken,
      rpId: window.location.hostname,
      createdAt: new Date().toISOString()
    };

    this.persistProfile(profile);
    return profile;
  }

  async authenticate(): Promise<StoredBiometricProfile> {
    const profile = this.getStoredProfile();
    if (!profile) {
      throw new Error('Nenhuma biometria configurada neste dispositivo.');
    }

    const challenge = this.randomBuffer(32);
    const publicKey: PublicKeyCredentialRequestOptions = {
      challenge,
      allowCredentials: [
        {
          id: this.base64UrlToBuffer(profile.credentialId),
          type: 'public-key',
          transports: ['internal']
        }
      ],
      userVerification: 'required',
      timeout: 60000,
      rpId: profile.rpId ?? window.location.hostname
    };

    const assertion = await navigator.credentials.get({ publicKey });
    if (!(assertion instanceof PublicKeyCredential)) {
      throw new Error('Falha ao validar a biometria.');
    }

    const response = assertion.response as AuthenticatorAssertionResponse;
    this.validateChallenge(response.clientDataJSON, challenge);
    this.ensureUserVerified(response.authenticatorData);

    return profile;
  }

  updateRefreshToken(refreshToken: string): void {
    const profile = this.getStoredProfile();
    if (!profile) {
      return;
    }

    profile.refreshToken = refreshToken;
    this.persistProfile(profile);
  }

  private persistProfile(profile: StoredBiometricProfile): void {
    localStorage.setItem(this.storageKey, JSON.stringify(profile));
  }

  private randomBuffer(length: number): ArrayBuffer {
    const buffer = new Uint8Array(length);
    crypto.getRandomValues(buffer);
    return buffer.buffer;
  }

  private async getUserId(email: string): Promise<ArrayBuffer> {
    const data = new TextEncoder().encode(email.toLowerCase());
    const hash = await crypto.subtle.digest('SHA-256', data);
    return hash;
  }

  private validateChallenge(clientDataJSON: ArrayBuffer, expectedChallenge: ArrayBuffer): void {
    const decoded = this.decodeClientDataJSON(clientDataJSON);
    const expected = this.bufferToBase64Url(expectedChallenge);

    if (decoded.type !== 'webauthn.get') {
      throw new Error('Resposta biométrica inesperada.');
    }

    if (this.normalizeBase64Url(decoded.challenge) !== this.normalizeBase64Url(expected)) {
      throw new Error('Desafio biométrico não confere.');
    }
  }

  private decodeClientDataJSON(buffer: ArrayBuffer): ClientDataJSON {
    const json = new TextDecoder().decode(buffer);
    return JSON.parse(json) as ClientDataJSON;
  }

  private ensureUserVerified(authenticatorData: ArrayBuffer): void {
    if (authenticatorData.byteLength < 33) {
      throw new Error('Dados de autenticação biométrica incompletos.');
    }

    const flags = new DataView(authenticatorData).getUint8(32);
    const userVerified = (flags & 0x04) === 0x04;

    if (!userVerified) {
      throw new Error('A biometria não confirmou o usuário.');
    }
  }

  private bufferToBase64Url(buffer: ArrayBuffer | Uint8Array): string {
    const bytes = buffer instanceof ArrayBuffer ? new Uint8Array(buffer) : buffer;
    let binary = '';
    for (let i = 0; i < bytes.byteLength; i++) {
      binary += String.fromCharCode(bytes[i]);
    }

    return btoa(binary)
      .replace(/\+/g, '-')
      .replace(/\//g, '_')
      .replace(/=+$/g, '');
  }

  private base64UrlToBuffer(base64Url: string): ArrayBuffer {
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const padded = base64.padEnd(base64.length + ((4 - (base64.length % 4)) % 4), '=');
    const decoded = atob(padded);
    const buffer = new Uint8Array(decoded.length);

    for (let i = 0; i < decoded.length; i++) {
      buffer[i] = decoded.charCodeAt(i);
    }

    return buffer.buffer;
  }

  private normalizeBase64Url(value: string): string {
    return value.replace(/\+/g, '-').replace(/\//g, '_').replace(/=+$/g, '');
  }
}
