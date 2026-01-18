import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { DeviceIdService } from './device-id.service';

export interface LoginRequest {
  email: string;
  password: string;
  deviceId?: string;
}

export interface LoginResponse {
  token: string;
  refreshToken: string;
}

export interface RefreshTokenRequest {
  refreshToken: string;
  deviceId?: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly baseUrl = '/api/auth';
  private readonly accessTokenKey = 'agroconnect.accessToken';
  private readonly refreshTokenKey = 'agroconnect.refreshToken';

  constructor(
    private readonly http: HttpClient,
    private readonly deviceIdService: DeviceIdService
  ) {}

  login(payload: LoginRequest): Observable<LoginResponse> {
    const deviceId = payload.deviceId || this.deviceIdService.getOrCreate();
    const body = { ...payload, deviceId };

    return this.http
      .post<LoginResponse>(`${this.baseUrl}/login`, body)
      .pipe(tap(({ token, refreshToken }) => this.persistTokens(token, refreshToken)));
  }

  logout(): void {
    localStorage.removeItem(this.accessTokenKey);
    localStorage.removeItem(this.refreshTokenKey);
  }

  refreshSession(refreshToken: string): Observable<LoginResponse> {
    const deviceId = this.deviceIdService.getOrCreate();
    return this.http
      .post<LoginResponse>(`${this.baseUrl}/refresh`, { refreshToken, deviceId })
      .pipe(tap(({ token, refreshToken: newRefreshToken }) => this.persistTokens(token, newRefreshToken)));
  }

  getStoredRefreshToken(): string | null {
    return localStorage.getItem(this.refreshTokenKey);
  }

  hasActiveSession(): boolean {
    return !!localStorage.getItem(this.accessTokenKey);
  }

  getAccessToken(): string | null {
    return localStorage.getItem(this.accessTokenKey);
  }

  private persistTokens(token: string, refreshToken: string): void {
    localStorage.setItem(this.accessTokenKey, token);
    localStorage.setItem(this.refreshTokenKey, refreshToken);
  }
}
