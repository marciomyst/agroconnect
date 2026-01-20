import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { map } from 'rxjs';
import { AuthTokenService } from '../auth/auth-token.service';
import { CurrentUserStore } from '../auth/current-user.store';

export const authCanMatch: CanMatchFn = () => {
  const store = inject(CurrentUserStore);
  const tokenService = inject(AuthTokenService);
  const router = inject(Router);

  if (!tokenService.hasToken()) {
    store.clear();
    return router.parseUrl('/login');
  }

  if (store.isLoaded()) {
    return store.isAuthenticated() ? true : router.parseUrl('/login');
  }

  return store.loadFromApi().pipe(
    map(context => (context ? true : router.parseUrl('/login')))
  );
};
