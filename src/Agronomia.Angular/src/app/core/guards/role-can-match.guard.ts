import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { AuthTokenService } from '../auth/auth-token.service';
import { CurrentUserStore } from '../auth/current-user.store';
import { OrganizationType } from '../auth/auth.types';
import { ActiveOrganizationStore } from '../organization/active-organization.store';

const resolveOrganizationType = (role: string): OrganizationType | null => {
  if (role === 'Seller') {
    return 'Seller';
  }

  if (role === 'Farmer') {
    return 'Farm';
  }

  if (role === 'Farm') {
    return 'Farm';
  }

  return null;
};

export const roleCanMatch = (requiredRole: string): CanMatchFn => {
  return () => {
    const currentUserStore = inject(CurrentUserStore);
    const activeOrganizationStore = inject(ActiveOrganizationStore);
    const tokenService = inject(AuthTokenService);
    const router = inject(Router);

    if (!tokenService.hasToken()) {
      currentUserStore.clear();
      return router.parseUrl('/login');
    }

    activeOrganizationStore.loadFromStorage();
    const activeOrganization = activeOrganizationStore.activeOrganization();

    if (!activeOrganization) {
      return router.parseUrl('/select-organization');
    }

    const organizationType = resolveOrganizationType(requiredRole);

    if (organizationType) {
      return activeOrganization.organizationType === organizationType;
    }

    return activeOrganization.roles.includes(requiredRole);
  };
};
