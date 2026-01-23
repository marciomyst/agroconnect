import { CommonModule } from '@angular/common';
import { Component, DestroyRef, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MarketplaceApiService, MarketplaceProductOfferItem, ProductDetails } from '../../marketplace/services/marketplace-api.service';
import { PurchaseIntentApiService } from './purchase-intent-api.service';

@Component({
  selector: 'app-purchase-intent-create-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './purchase-intent-create-page.component.html',
  styleUrls: ['./purchase-intent-create-page.component.scss'],
})
export class PurchaseIntentCreatePageComponent {
  private readonly marketplaceApi = inject(MarketplaceApiService);
  private readonly purchaseIntentApi = inject(PurchaseIntentApiService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly formBuilder = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);

  readonly isLoading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly product = signal<ProductDetails | null>(null);
  readonly selectedOffer = signal<MarketplaceProductOfferItem | null>(null);

  readonly form = this.formBuilder.nonNullable.group({
    quantity: [1, [Validators.required, Validators.min(0.01)]],
    notes: [''],
  });

  constructor() {
    const productId = this.route.snapshot.paramMap.get('productId');
    const sellerProductId = this.route.snapshot.queryParamMap.get('sellerProductId');

    if (!productId || !sellerProductId) {
      this.errorMessage.set('Missing product or seller information.');
      return;
    }

    this.isLoading.set(true);
    this.marketplaceApi.getProduct(productId).pipe(
      finalize(() => this.isLoading.set(false)),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe({
      next: product => this.product.set(product),
      error: () => this.errorMessage.set('Failed to load product.'),
    });

    this.marketplaceApi.getProductOffers(productId).pipe(
      takeUntilDestroyed(this.destroyRef)
    ).subscribe({
      next: offers => {
        const match = offers.find(offer => offer.sellerProductId === sellerProductId) ?? null;
        this.selectedOffer.set(match);
      },
      error: () => {
        this.selectedOffer.set(null);
      },
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const sellerProductId = this.route.snapshot.queryParamMap.get('sellerProductId');
    if (!sellerProductId) {
      this.errorMessage.set('Missing seller product.');
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.purchaseIntentApi.createPurchaseIntent({
      sellerProductId,
      quantity: Number(this.form.controls.quantity.value),
      notes: this.form.controls.notes.value || null,
    }).pipe(
      finalize(() => this.isLoading.set(false)),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe({
      next: () => {
        void this.router.navigate(['/farmer/purchase-intents']);
      },
      error: () => {
        this.errorMessage.set('Failed to create purchase intent.');
      },
    });
  }
}
