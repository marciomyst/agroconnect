import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthTokenService } from './auth-token.service';
import { ActiveOrganizationStore } from '../organization/active-organization.store';

const apiPrefix = '/api/';

// Propagate active organization context for authenticated API calls.
const getOrganizationHeaders = (organizationId: string | null): Record<string, string> => {
  if (!organizationId) {
    return {};
  }

  return {
    'X-Organization-Id': organizationId,
  };
};

const isApiRequest = (url: string): boolean => {
  if (url.startsWith(apiPrefix) || url.startsWith('api/')) {
    return true;
  }

  try {
    const parsed = new URL(url);
    return parsed.origin === window.location.origin
      && parsed.pathname.startsWith(apiPrefix);
  } catch {
    return false;
  }
};

export const authTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const tokenService = inject(AuthTokenService);
  const token = tokenService.getToken();

  if (!token || !isApiRequest(req.url)) {
    return next(req);
  }

  const activeOrganizationStore = inject(ActiveOrganizationStore);
  const authRequest = req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`,
    },
  });

  const organizationId = activeOrganizationStore.activeOrganization()?.organizationId ?? null;
  const organizationHeaders = getOrganizationHeaders(organizationId);

  const requestWithContext = Object.keys(organizationHeaders).length
    ? authRequest.clone({ setHeaders: organizationHeaders })
    : authRequest;

  return next(requestWithContext);
};
