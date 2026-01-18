import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export type CreateSellerPayload = {
  cnpj: string;
  stateRegistration: string;
  legalName: string;
  tradeName: string;
  contactEmail: string;
  contactPhone: string;
  responsibleName: string;
  zipCode: string;
  street: string;
  number: string;
  city: string;
  state: string;
  complement?: string;
  password: string;
};

export type CreateSellerResponse = {
  id: string;
};

@Injectable({
  providedIn: 'root'
})
export class SellerApiService {
  constructor(private readonly http: HttpClient) {}

  createSeller(payload: CreateSellerPayload): Observable<CreateSellerResponse> {
    return this.http.post<CreateSellerResponse>('/api/sellers', payload);
  }
}
