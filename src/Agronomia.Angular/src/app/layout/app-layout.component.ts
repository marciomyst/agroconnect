import { CommonModule } from '@angular/common';
import { Component, computed, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthTokenService } from '../core/auth/auth-token.service';
import { CurrentUserStore } from '../core/auth/current-user.store';
import { ActiveOrganizationStore } from '../core/organization/active-organization.store';

interface NavItem {
  label: string;
  route: string;
  exact: boolean;
}

interface NavSection {
  title: string;
  items: NavItem[];
}

const ADMIN_NAV: NavSection = {
  title: 'Admin',
  items: [
    { label: 'Dashboard', route: '/admin', exact: true },
  ],
};

const SELLER_NAV: NavSection = {
  title: 'Seller',
  items: [
    { label: 'Dashboard', route: '/seller', exact: true },
    { label: 'Catalog', route: '/seller/catalog', exact: false },
    { label: 'Members', route: '/seller/members', exact: false },
  ],
};

const FARMER_NAV: NavSection = {
  title: 'Farmer',
  items: [
    { label: 'Dashboard', route: '/farmer', exact: true },
    { label: 'Marketplace', route: '/farmer/marketplace', exact: false },
    { label: 'Purchase Intents', route: '/farmer/purchase-intents', exact: false },
    { label: 'Members', route: '/farmer/members', exact: false },
  ],
};

@Component({
  selector: 'app-app-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app-layout.component.html',
  styleUrls: ['./app-layout.component.scss'],
})
export class AppLayoutComponent {
  private readonly router = inject(Router);
  private readonly tokenService = inject(AuthTokenService);
  private readonly currentUserStore = inject(CurrentUserStore);
  private readonly activeOrganizationStore = inject(ActiveOrganizationStore);

  // Context consumed by the topbar and menu.
  readonly user = this.currentUserStore.user;
  readonly activeOrganization = this.activeOrganizationStore.activeOrganization;

  readonly navSections = computed(() => {
    const sections: NavSection[] = [];
    const activeOrganization = this.activeOrganization();

    if (this.currentUserStore.hasRole('Admin')) {
      sections.push(ADMIN_NAV);
    }

    if (activeOrganization?.organizationType === 'Seller') {
      sections.push(SELLER_NAV);
    }

    if (activeOrganization?.organizationType === 'Farm') {
      sections.push(FARMER_NAV);
    }

    return sections;
  });

  onChangeOrganization(): void {
    void this.router.navigate(['/select-organization']);
  }

  onLogout(): void {
    this.tokenService.clearToken();
    this.currentUserStore.clear();
    this.activeOrganizationStore.clear();
    void this.router.navigate(['/login']);
  }
}
