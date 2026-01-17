import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  refreshToken: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly baseUrl = '/api/auth';
  private readonly accessTokenKey = 'agroconnect.accessToken';
  private readonly refreshTokenKey = 'agroconnect.refreshToken';

  constructor(private readonly http: HttpClient) {}

  login(payload: LoginRequest): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${this.baseUrl}/login`, payload)
      .pipe(tap(({ token, refreshToken }) => this.persistTokens(token, refreshToken)));
  }

  logout(): void {
    localStorage.removeItem(this.accessTokenKey);
    localStorage.removeItem(this.refreshTokenKey);
  }

  hasActiveSession(): boolean {
    return !!localStorage.getItem(this.accessTokenKey);
  }

  private persistTokens(token: string, refreshToken: string): void {
    localStorage.setItem(this.accessTokenKey, token);
    localStorage.setItem(this.refreshTokenKey, refreshToken);
  }
}
