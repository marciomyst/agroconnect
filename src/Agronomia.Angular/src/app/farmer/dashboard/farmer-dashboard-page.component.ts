import { CommonModule } from '@angular/common';
import { Component, computed, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CurrentUserStore } from '../../core/auth/current-user.store';
import { ActiveOrganizationStore } from '../../core/organization/active-organization.store';
import { CollectiveDealsClosingComponent } from '../../shared/components/collective-deals-closing/collective-deals-closing.component';

@Component({
  selector: 'app-farmer-dashboard-page',
  standalone: true,
  imports: [CommonModule, RouterLink, CollectiveDealsClosingComponent],
  templateUrl: './farmer-dashboard-page.component.html',
  styleUrls: ['./farmer-dashboard-page.component.scss'],
})
export class FarmerDashboardPageComponent {
  private readonly currentUserStore = inject(CurrentUserStore);
  private readonly activeOrganizationStore = inject(ActiveOrganizationStore);

  readonly userDisplayName = computed(() => {
    const user = this.currentUserStore.user();
    return user?.name || user?.email || 'Farm';
  });

  readonly organizationName = computed(() => {
    return this.activeOrganizationStore.activeOrganization()?.organizationName || 'Farm workspace';
  });
}
