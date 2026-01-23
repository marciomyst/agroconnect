import { CommonModule } from '@angular/common';
import { Component, DestroyRef, effect, inject, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { debounceTime, distinctUntilChanged, finalize, Subscription } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActiveOrganizationStore } from '../../../core/organization/active-organization.store';
import { PagedResult } from '../../../shared/models/paged-result';
import { SellerCatalogApiService, SellerCatalogItem } from '../services/seller-catalog-api.service';

@Component({
  selector: 'app-seller-catalog-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './seller-catalog-page.component.html',
  styleUrls: ['./seller-catalog-page.component.scss'],
})
export class SellerCatalogPageComponent {
  private readonly catalogApi = inject(SellerCatalogApiService);
  private readonly organizationStore = inject(ActiveOrganizationStore);
  private readonly destroyRef = inject(DestroyRef);

  private loadSubscription?: Subscription;

  readonly searchControl = new FormControl('', { nonNullable: true });
  readonly catalog = signal<PagedResult<SellerCatalogItem> | null>(null);
  readonly isLoading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly page = signal(1);
  readonly pageSize = signal(10);

  constructor() {
    this.searchControl.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe(value => {
      this.page.set(1);
      this.loadCatalog(value);
    });

    effect(() => {
      const organization = this.organizationStore.activeOrganization();
      if (!organization) {
        this.catalog.set(null);
        return;
      }

      if (organization.organizationType !== 'Seller') {
        this.errorMessage.set('Seller context required.');
        return;
      }

      this.page.set(1);
      this.loadCatalog(this.searchControl.value);
    });
  }

  get totalPages(): number {
    const result = this.catalog();
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
    this.loadCatalog(this.searchControl.value);
  }

  previousPage(): void {
    if (this.page() <= 1) {
      return;
    }
    this.page.update(value => value - 1);
    this.loadCatalog(this.searchControl.value);
  }

  private loadCatalog(search?: string): void {
    const organization = this.organizationStore.activeOrganization();
    if (!organization) {
      return;
    }

    this.loadSubscription?.unsubscribe();
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.loadSubscription = this.catalogApi.getCatalog(
      organization.organizationId,
      this.page(),
      this.pageSize(),
      search?.trim() || undefined
    ).pipe(
      finalize(() => this.isLoading.set(false)),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe({
      next: result => this.catalog.set(result),
      error: () => {
        this.errorMessage.set('Failed to load catalog. Please try again.');
      },
    });
  }
}
