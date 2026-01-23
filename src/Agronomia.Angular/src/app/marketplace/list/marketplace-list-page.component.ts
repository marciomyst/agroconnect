import { CommonModule } from '@angular/common';
import { Component, DestroyRef, inject, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { debounceTime, distinctUntilChanged, finalize } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MarketplaceApiService, MarketplaceProductListItem } from '../services/marketplace-api.service';
import { PagedResult } from '../../shared/models/paged-result';

interface CategoryOption {
  value: string;
  label: string;
}

const CATEGORY_OPTIONS: CategoryOption[] = [
  { value: '', label: 'All categories' },
  { value: 'CropProtection', label: 'Crop Protection' },
  { value: 'Fertilizer', label: 'Fertilizer' },
  { value: 'Seed', label: 'Seed' },
  { value: 'Inoculant', label: 'Inoculant' },
  { value: 'Biological', label: 'Biological' },
  { value: 'Adjuvant', label: 'Adjuvant' },
  { value: 'Other', label: 'Other' },
];

@Component({
  selector: 'app-marketplace-list-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './marketplace-list-page.component.html',
  styleUrls: ['./marketplace-list-page.component.scss'],
})
export class MarketplaceListPageComponent {
  private readonly marketplaceApi = inject(MarketplaceApiService);
  private readonly destroyRef = inject(DestroyRef);

  readonly searchControl = new FormControl('', { nonNullable: true });
  readonly categoryControl = new FormControl('', { nonNullable: true });

  readonly products = signal<PagedResult<MarketplaceProductListItem> | null>(null);
  readonly isLoading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly page = signal(1);
  readonly pageSize = signal(12);

  readonly categories = CATEGORY_OPTIONS;

  constructor() {
    this.searchControl.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe(() => {
      this.page.set(1);
      this.loadProducts();
    });

    this.categoryControl.valueChanges.pipe(
      distinctUntilChanged(),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe(() => {
      this.page.set(1);
      this.loadProducts();
    });

    this.loadProducts();
  }

  get totalPages(): number {
    const result = this.products();
    if (!result) {
      return 1;
    }
    return Math.max(1, Math.ceil(result.totalCount / result.pageSize));
  }

  nextPage(): void {
    if (this.page() >= this.totalPages) {
      return;
    }
    this.page.update(value => value + 1);
    this.loadProducts();
  }

  previousPage(): void {
    if (this.page() <= 1) {
      return;
    }
    this.page.update(value => value - 1);
    this.loadProducts();
  }

  private loadProducts(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.marketplaceApi.getProducts(
      this.searchControl.value.trim(),
      this.categoryControl.value,
      this.page(),
      this.pageSize()
    ).pipe(
      finalize(() => this.isLoading.set(false)),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe({
      next: result => this.products.set(result),
      error: () => this.errorMessage.set('Failed to load marketplace products.'),
    });
  }
}
