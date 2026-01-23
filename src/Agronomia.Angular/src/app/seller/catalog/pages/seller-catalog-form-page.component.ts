import { CommonModule } from '@angular/common';
import { Component, DestroyRef, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { debounceTime, distinctUntilChanged, finalize } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActiveOrganizationStore } from '../../../core/organization/active-organization.store';
import { SellerCatalogApiService, ProductListItem } from '../services/seller-catalog-api.service';

@Component({
  selector: 'app-seller-catalog-form-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './seller-catalog-form-page.component.html',
  styleUrls: ['./seller-catalog-form-page.component.scss'],
})
export class SellerCatalogFormPageComponent {
  private readonly catalogApi = inject(SellerCatalogApiService);
  private readonly organizationStore = inject(ActiveOrganizationStore);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly formBuilder = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);

  readonly isLoading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly isEditMode = signal(false);
  readonly selectedProduct = signal<ProductListItem | null>(null);
  readonly productResults = signal<ProductListItem[]>([]);

  readonly productSearchControl = this.formBuilder.nonNullable.control('');

  readonly form = this.formBuilder.nonNullable.group({
    productId: ['', Validators.required],
    price: [0, [Validators.required, Validators.min(0.01)]],
    isAvailable: [true],
  });

  constructor() {
    const sellerProductId = this.route.snapshot.paramMap.get('sellerProductId');
    if (sellerProductId) {
      this.isEditMode.set(true);
      this.loadExistingSellerProduct(sellerProductId);
    }

    this.productSearchControl.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe(term => this.searchProducts(term));
  }

  submit(): void {
    if (this.isLoading()) {
      return;
    }

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const organization = this.organizationStore.activeOrganization();
    if (!organization) {
      this.errorMessage.set('Active organization required.');
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);

    const payload = {
      productId: this.form.controls.productId.value,
      price: Number(this.form.controls.price.value),
      currency: 'BRL',
      isAvailable: this.form.controls.isAvailable.value,
    };

    const sellerProductId = this.route.snapshot.paramMap.get('sellerProductId');

    const request$ = sellerProductId
      ? this.catalogApi.updateSellerProduct(organization.organizationId, sellerProductId, payload)
      : this.catalogApi.createSellerProduct(organization.organizationId, payload);

    request$.pipe(
      finalize(() => this.isLoading.set(false)),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe({
      next: () => {
        void this.router.navigate(['/seller/catalog']);
      },
      error: error => {
        if (error?.status === 409) {
          this.errorMessage.set('This product is already in your catalog.');
          return;
        }
        this.errorMessage.set('Failed to save catalog item. Please try again.');
      },
    });
  }

  cancel(): void {
    void this.router.navigate(['/seller/catalog']);
  }

  selectProduct(product: ProductListItem): void {
    this.selectedProduct.set(product);
    this.form.controls.productId.setValue(product.productId);
    this.productResults.set([]);
    this.productSearchControl.setValue(product.name, { emitEvent: false });
  }

  private searchProducts(term: string): void {
    if (this.isEditMode()) {
      return;
    }

    const search = term.trim();
    if (search.length < 2) {
      this.productResults.set([]);
      return;
    }

    this.catalogApi.searchProducts(search).pipe(
      takeUntilDestroyed(this.destroyRef)
    ).subscribe({
      next: result => this.productResults.set(result.items),
      error: () => this.productResults.set([]),
    });
  }

  private loadExistingSellerProduct(sellerProductId: string): void {
    const organization = this.organizationStore.activeOrganization();
    if (!organization) {
      this.errorMessage.set('Active organization required.');
      return;
    }

    this.isLoading.set(true);

    this.catalogApi.getCatalog(organization.organizationId, 1, 200).pipe(
      finalize(() => this.isLoading.set(false)),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe({
      next: result => {
        const match = result.items.find(item => item.sellerProductId === sellerProductId);
        if (!match) {
          this.errorMessage.set('Catalog item not found.');
          return;
        }

        this.form.patchValue({
          productId: match.productId,
          price: match.price,
          isAvailable: match.isAvailable,
        });

        const product: ProductListItem = {
          productId: match.productId,
          name: match.productName,
          category: match.category,
          unitOfMeasure: match.unitOfMeasure,
          isControlledByRecipe: match.isControlledByRecipe,
          isActive: true,
          registrationNumber: null,
        };
        this.selectedProduct.set(product);
        this.productSearchControl.setValue(match.productName, { emitEvent: false });
      },
      error: () => {
        this.errorMessage.set('Failed to load catalog item.');
      },
    });
  }
}
