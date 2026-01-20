import { inject } from '@angular/core';
import { CanMatchFn } from '@angular/router';
import { CurrentUserStore } from '../auth/current-user.store';
import { OrganizationType } from '../auth/auth.types';
import { ActiveOrganizationStore } from '../organization/active-organization.store';
import { ActiveOrganization } from '../organization/organization.model';

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

const hasRoleInActiveOrganization = (
  requiredRole: string,
  currentUserStore: CurrentUserStore,
  activeOrganization: ActiveOrganization
): boolean => {
  const user = currentUserStore.user();
  if (!user) {
    return false;
  }

  const organization = user.organizations.find(org =>
    org.organizationId === activeOrganization.organizationId
    && org.type === activeOrganization.organizationType
  );

  return organization ? organization.roles.includes(requiredRole) : false;
};

export const roleCanMatch = (requiredRole: string): CanMatchFn => {
  return () => {
    const currentUserStore = inject(CurrentUserStore);
    const activeOrganizationStore = inject(ActiveOrganizationStore);
    const activeOrganization = activeOrganizationStore.activeOrganization();
    const requiredOrganizationType = resolveOrganizationType(requiredRole);

    // Role guard: organization-type roles require an active organization context.
    if (requiredOrganizationType) {
      return activeOrganization
        ? activeOrganization.organizationType === requiredOrganizationType
        : false;
    }

    // Role guard: validate scoped roles against the active organization.
    if (activeOrganization) {
      return hasRoleInActiveOrganization(requiredRole, currentUserStore, activeOrganization);
    }

    // No active organization: fall back to global roles (e.g., admin).
    return currentUserStore.hasRole(requiredRole);
  };
};
