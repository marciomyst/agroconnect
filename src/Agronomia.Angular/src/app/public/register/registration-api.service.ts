import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface RegisterUserRequest {
  name: string;
  email: string;
  password: string;
}

export interface RegisterUserResponse {
  userId: string;
  email: string;
}

export interface RegisterFarmRequest {
  taxId: string;
  name: string;
}

export interface RegisterFarmResponse {
  farmId: string;
  farmMembershipId: string;
}

export interface RegisterSellerRequest {
  taxId: string;
  corporateName: string;
}

export interface RegisterSellerResponse {
  sellerId: string;
  sellerMembershipId: string;
}

@Injectable({ providedIn: 'root' })
export class RegistrationApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/api`;

  registerUser(payload: RegisterUserRequest): Observable<RegisterUserResponse> {
    return this.http.post<RegisterUserResponse>(`${this.baseUrl}/users/register`, payload);
  }

  registerFarm(payload: RegisterFarmRequest): Observable<RegisterFarmResponse> {
    return this.http.post<RegisterFarmResponse>(`${this.baseUrl}/farms`, payload);
  }

  registerSeller(payload: RegisterSellerRequest): Observable<RegisterSellerResponse> {
    return this.http.post<RegisterSellerResponse>(`${this.baseUrl}/sellers`, payload);
  }
}
