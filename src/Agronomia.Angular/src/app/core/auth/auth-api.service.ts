import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CurrentUserContext, LoginRequest, LoginResponse } from './auth.types';

@Injectable({ providedIn: 'root' })
export class AuthApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/auth';

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.baseUrl}/login`, request);
  }

  getCurrentUserContext(): Observable<CurrentUserContext> {
    return this.http.get<CurrentUserContext>(`${this.baseUrl}/me`);
  }
}
