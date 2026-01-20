import { computed, effect, inject, Injectable, signal } from '@angular/core';
import { CurrentUserStore } from '../auth/current-user.store';
import { CurrentUserContext, CurrentUserOrganization } from '../auth/auth.types';
import { ActiveOrganization } from './organization.model';

@Injectable({ providedIn: 'root' })
export class ActiveOrganizationStore {
  private readonly currentUserStore = inject(CurrentUserStore);
  private readonly storageKey = 'agroconnect.active-organization';

  private readonly _activeOrganization = signal<ActiveOrganization | null>(null);
  private readonly _restoredForUserId = signal<string | null>(null);

  readonly activeOrganization = this._activeOrganization.asReadonly();
  readonly hasActiveOrganization = computed(() => this._activeOrganization() !== null);
  readonly isFarmContext = computed(() => this._activeOrganization()?.organizationType === 'Farm');
  readonly isSellerContext = computed(() => this._activeOrganization()?.organizationType === 'Seller');

  constructor() {
    effect(() => {
      const user = this.currentUserStore.user();
      const isUserLoaded = this.currentUserStore.isLoaded();
      const active = this._activeOrganization();

      if (!user) {
        if (isUserLoaded && active) {
          this.clear();
        }
        if (isUserLoaded) {
          this._restoredForUserId.set(null);
        }
        return;
      }

      if (this._restoredForUserId() !== user.userId) {
        this._restoredForUserId.set(user.userId);
        this.loadFromStorage();
        return;
      }

      if (active && !this.isOrganizationValid(user, active)) {
        this.clear();
        return;
      }

      if (!active && user.organizations.length === 1) {
        this.setActiveOrganization(this.toActiveOrganization(user.organizations[0]));
      }
    });
  }

  setActiveOrganization(organization: ActiveOrganization): void {
    const user = this.currentUserStore.user();
    if (!user || !this.isOrganizationValid(user, organization)) {
      this.clear();
      return;
    }

    this._activeOrganization.set(organization);
    localStorage.setItem(this.storageKey, JSON.stringify(organization));
  }

  clear(): void {
    this._activeOrganization.set(null);
    localStorage.removeItem(this.storageKey);
  }

  loadFromStorage(): void {
    const user = this.currentUserStore.user();
    if (!user) {
      if (this.currentUserStore.isLoaded()) {
        this.clear();
      }
      return;
    }

    const stored = localStorage.getItem(this.storageKey);
    if (!stored) {
      if (user.organizations.length === 1) {
        this.setActiveOrganization(this.toActiveOrganization(user.organizations[0]));
      }
      return;
    }

    try {
      const parsed = JSON.parse(stored) as ActiveOrganization;
      const match = this.findOrganization(user, parsed.organizationId, parsed.organizationType);

      if (!match) {
        this.clear();
        return;
      }

      this.setActiveOrganization(this.toActiveOrganization(match));
    } catch {
      this.clear();
    }
  }

  private isOrganizationValid(
    user: CurrentUserContext,
    organization: ActiveOrganization
  ): boolean {
    return !!this.findOrganization(user, organization.organizationId, organization.organizationType);
  }

  private findOrganization(
    user: CurrentUserContext,
    organizationId: string,
    organizationType: ActiveOrganization['organizationType']
  ): CurrentUserOrganization | undefined {
    return user.organizations.find(org =>
      org.organizationId === organizationId && org.type === organizationType
    );
  }

  private toActiveOrganization(organization: CurrentUserOrganization): ActiveOrganization {
    return {
      organizationId: organization.organizationId,
      organizationName: organization.organizationName,
      organizationType: organization.type,
      roles: organization.roles,
    };
  }
}
