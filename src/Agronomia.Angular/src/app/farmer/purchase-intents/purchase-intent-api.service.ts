import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface PurchaseIntentCreateRequest {
  sellerProductId: string;
  quantity: number;
  notes?: string | null;
}

export interface FarmPurchaseIntent {
  purchaseIntentId: string;
  productId: string;
  productName: string;
  sellerId: string;
  sellerName: string;
  sellerProductId: string;
  quantity: number;
  status: string;
  requestedAtUtc: string;
}

@Injectable({ providedIn: 'root' })
export class PurchaseIntentApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/api`;

  createPurchaseIntent(payload: PurchaseIntentCreateRequest): Observable<{ purchaseIntentId: string }> {
    return this.http.post<{ purchaseIntentId: string }>(`${this.baseUrl}/purchase-intents`, payload);
  }

  getMyPurchaseIntents(): Observable<FarmPurchaseIntent[]> {
    return this.http.get<FarmPurchaseIntent[]>(`${this.baseUrl}/me/purchase-intents`);
  }
}
