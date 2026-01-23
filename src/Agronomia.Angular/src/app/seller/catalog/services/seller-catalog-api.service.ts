import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { PagedResult } from '../../../shared/models/paged-result';

export interface SellerCatalogItem {
  sellerProductId: string;
  productId: string;
  productName: string;
  category: string;
  unitOfMeasure: string;
  price: number;
  currency: string;
  isAvailable: boolean;
  isControlledByRecipe: boolean;
}

export interface ProductListItem {
  productId: string;
  name: string;
  category: string;
  unitOfMeasure: string;
  registrationNumber?: string | null;
  isControlledByRecipe: boolean;
  isActive: boolean;
}

export interface SellerProductUpsertRequest {
  productId: string;
  price: number;
  currency: string;
  isAvailable: boolean;
}

@Injectable({ providedIn: 'root' })
export class SellerCatalogApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/api/sellers`;
  private readonly productsUrl = `${environment.apiUrl}/api/products`;

  getCatalog(
    sellerId: string,
    page: number,
    pageSize: number,
    search?: string
  ): Observable<PagedResult<SellerCatalogItem>> {
    let params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    if (search) {
      params = params.set('search', search);
    }

    return this.http.get<PagedResult<SellerCatalogItem>>(
      `${this.baseUrl}/${sellerId}/catalog`,
      { params }
    );
  }

  createSellerProduct(sellerId: string, payload: SellerProductUpsertRequest): Observable<{ sellerProductId: string }> {
    return this.http.post<{ sellerProductId: string }>(
      `${this.baseUrl}/${sellerId}/catalog`,
      payload
    );
  }

  updateSellerProduct(
    sellerId: string,
    sellerProductId: string,
    payload: SellerProductUpsertRequest
  ): Observable<{ sellerProductId: string }> {
    return this.http.put<{ sellerProductId: string }>(
      `${this.baseUrl}/${sellerId}/catalog/${sellerProductId}`,
      payload
    );
  }

  searchProducts(search: string, pageSize = 20): Observable<PagedResult<ProductListItem>> {
    let params = new HttpParams()
      .set('page', 1)
      .set('pageSize', pageSize);

    if (search) {
      params = params.set('search', search);
    }

    return this.http.get<PagedResult<ProductListItem>>(this.productsUrl, { params });
  }
}
