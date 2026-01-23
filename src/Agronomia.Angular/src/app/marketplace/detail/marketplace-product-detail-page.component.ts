import { CommonModule } from '@angular/common';
import { Component, DestroyRef, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { forkJoin, finalize } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MarketplaceApiService, MarketplaceProductOfferItem, ProductDetails } from '../services/marketplace-api.service';

@Component({
  selector: 'app-marketplace-product-detail-page',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './marketplace-product-detail-page.component.html',
  styleUrls: ['./marketplace-product-detail-page.component.scss'],
})
export class MarketplaceProductDetailPageComponent {
  private readonly marketplaceApi = inject(MarketplaceApiService);
  private readonly route = inject(ActivatedRoute);
  private readonly destroyRef = inject(DestroyRef);

  readonly product = signal<ProductDetails | null>(null);
  readonly offers = signal<MarketplaceProductOfferItem[]>([]);
  readonly isLoading = signal(false);
  readonly errorMessage = signal<string | null>(null);

  constructor() {
    const productId = this.route.snapshot.paramMap.get('productId');
    if (productId) {
      this.load(productId);
    } else {
      this.errorMessage.set('Product not found.');
    }
  }

  private load(productId: string): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    forkJoin({
      product: this.marketplaceApi.getProduct(productId),
      offers: this.marketplaceApi.getProductOffers(productId),
    }).pipe(
      finalize(() => this.isLoading.set(false)),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe({
      next: result => {
        this.product.set(result.product);
        this.offers.set(result.offers);
      },
      error: () => {
        this.errorMessage.set('Failed to load product details.');
      },
    });
  }
}
