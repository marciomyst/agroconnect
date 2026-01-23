import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResult } from '../../shared/models/paged-result';

export interface MarketplaceProductListItem {
  productId: string;
  name: string;
  category: string;
  isControlledByRecipe: boolean;
  bestPrice: number;
  hasAvailableOffers: boolean;
}

export interface MarketplaceProductOfferItem {
  sellerProductId: string;
  sellerId: string;
  sellerName: string;
  price: number;
  currency: string;
  isAvailable: boolean;
}

export interface ProductDetails {
  productId: string;
  name: string;
  category: string;
  unitOfMeasure: string;
  registrationNumber?: string | null;
  isControlledByRecipe: boolean;
  isActive: boolean;
  createdAtUtc: string;
}

@Injectable({ providedIn: 'root' })
export class MarketplaceApiService {
  private readonly http = inject(HttpClient);
  private readonly marketplaceUrl = `${environment.apiUrl}/api/marketplace`;
  private readonly productsUrl = `${environment.apiUrl}/api/products`;

  getProducts(
    search: string,
    category: string,
    page: number,
    pageSize: number
  ): Observable<PagedResult<MarketplaceProductListItem>> {
    let params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    if (search) {
      params = params.set('search', search);
    }

    if (category) {
      params = params.set('category', category);
    }

    return this.http.get<PagedResult<MarketplaceProductListItem>>(
      `${this.marketplaceUrl}/products`,
      { params }
    );
  }

  getProductOffers(productId: string): Observable<MarketplaceProductOfferItem[]> {
    return this.http.get<MarketplaceProductOfferItem[]>(
      `${this.marketplaceUrl}/products/${productId}/offers`
    );
  }

  getProduct(productId: string): Observable<ProductDetails> {
    return this.http.get<ProductDetails>(`${this.productsUrl}/${productId}`);
  }
}
