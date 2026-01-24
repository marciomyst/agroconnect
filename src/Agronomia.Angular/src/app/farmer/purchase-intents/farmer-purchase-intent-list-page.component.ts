import { CommonModule } from '@angular/common';
import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FarmPurchaseIntent, PurchaseIntentApiService } from './purchase-intent-api.service';

@Component({
  standalone: true,
  selector: 'app-farmer-purchase-intent-list-page',
  imports: [CommonModule, RouterModule],
  templateUrl: './farmer-purchase-intent-list-page.component.html',
  styleUrls: ['./farmer-purchase-intent-list-page.component.scss'],
})
export class FarmerPurchaseIntentListPageComponent implements OnInit {
  private readonly api = inject(PurchaseIntentApiService);

  readonly isLoading = signal(true);
  readonly errorMessage = signal<string | null>(null);
  readonly intents = signal<FarmPurchaseIntent[]>([]);

  readonly hasIntents = computed(
    () => !this.isLoading() && this.errorMessage() === null && this.intents().length > 0,
  );

  readonly isEmpty = computed(
    () => !this.isLoading() && this.errorMessage() === null && this.intents().length === 0,
  );

  ngOnInit(): void {
    this.loadIntents();
  }

  loadIntents(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.api.getMyPurchaseIntents().subscribe({
      next: (response: FarmPurchaseIntent[]) => {
        this.intents.set(response);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Failed to load purchase intents', error);
        this.errorMessage.set('Failed to load purchase intents. Please try again.');
        this.isLoading.set(false);
      },
    });
  }

  getStatusBadgeClass(status: string): string {
    switch (status) {
      case 'Pending':
        return 'status-badge status-badge--pending';
      case 'Accepted':
        return 'status-badge status-badge--accepted';
      case 'Rejected':
        return 'status-badge status-badge--rejected';
      case 'Expired':
        return 'status-badge status-badge--expired';
      default:
        return 'status-badge';
    }
  }

  formatDate(iso: string): string {
    if (!iso) {
      return '';
    }

    const date = new Date(iso);
    if (Number.isNaN(date.getTime())) {
      return iso;
    }

    return date.toLocaleString();
  }
}
