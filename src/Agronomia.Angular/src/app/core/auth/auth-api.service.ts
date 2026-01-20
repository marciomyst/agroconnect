import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ChangePasswordRequest, CurrentUserContext, LoginRequest, LoginResponse } from './auth.types';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/api/auth`;

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.baseUrl}/authenticate`, request);
  }

  getCurrentUserContext(): Observable<CurrentUserContext> {
    return this.http.get<CurrentUserContext>(`${this.baseUrl}/me`);
  }

  changePassword(request: ChangePasswordRequest): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/change-password`, request);
  }
}
