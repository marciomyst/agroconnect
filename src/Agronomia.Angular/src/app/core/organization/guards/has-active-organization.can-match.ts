import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { ActiveOrganizationStore } from '../active-organization.store';

export const hasActiveOrganizationCanMatch: CanMatchFn = () => {
  const activeOrganizationStore = inject(ActiveOrganizationStore);
  const router = inject(Router);

  activeOrganizationStore.loadFromStorage();

  return activeOrganizationStore.hasActiveOrganization()
    ? true
    : router.parseUrl('/select-organization');
};
