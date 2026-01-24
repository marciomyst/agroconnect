import { inject } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { CanMatchFn, Router } from '@angular/router';
import { filter, map, take } from 'rxjs';
import { ActiveOrganizationStore } from '../active-organization.store';
import { CurrentUserStore } from '../../auth/current-user.store';

export const hasActiveOrganizationCanMatch: CanMatchFn = () => {
  const activeOrganizationStore = inject(ActiveOrganizationStore);
  const currentUserStore = inject(CurrentUserStore);
  const router = inject(Router);

  const resolveActiveOrganization = () => {
    if (activeOrganizationStore.hasActiveOrganization()) {
      return true;
    }

    activeOrganizationStore.loadFromStorage();

    return activeOrganizationStore.hasActiveOrganization()
      ? true
      : router.parseUrl('/select-organization');
  };

  // Organization guard: require an active organization or redirect to selection.
  if (currentUserStore.isLoaded()) {
    return resolveActiveOrganization();
  }

  return toObservable(currentUserStore.isLoaded).pipe(
    filter(isLoaded => isLoaded),
    take(1),
    map(() => resolveActiveOrganization())
  );
};
