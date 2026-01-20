import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { AuthTokenService } from '../auth/auth-token.service';
import { CurrentUserStore } from '../auth/current-user.store';
import { ActiveOrganizationStore } from '../organization/active-organization.store';

const apiPrefix = '/api/';
const publicRoutes = ['/login', '/select-organization'];
let isNavigatingToLogin = false;

export interface NormalizedHttpError {
  status: number;
  code?: string;
  message: string;
}

export interface NormalizedValidationError extends NormalizedHttpError {
  fieldErrors?: Record<string, string[]>;
}

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

const isPublicRoute = (url: string): boolean => {
  const cleanUrl = url.split('?')[0];
  return publicRoutes.some(route => cleanUrl === route || cleanUrl.startsWith(`${route}/`));
};

const toStringArray = (value: unknown): string[] => {
  if (Array.isArray(value)) {
    return value.filter((item): item is string => typeof item === 'string' && item.length > 0);
  }

  if (typeof value === 'string' && value.length > 0) {
    return [value];
  }

  return [];
};

const mergeFieldErrors = (
  target: Record<string, string[]>,
  source: unknown
): void => {
  if (!source || typeof source !== 'object' || Array.isArray(source)) {
    return;
  }

  for (const [field, messages] of Object.entries(source)) {
    const normalized = toStringArray(messages);
    if (normalized.length > 0) {
      target[field] = normalized;
    }
  }
};

const resolveMessage = (
  error: HttpErrorResponse,
  payload: unknown,
  fallback: string
): string => {
  if (typeof payload === 'string' && payload.length > 0) {
    return payload;
  }

  if (payload && typeof payload === 'object' && !Array.isArray(payload)) {
    const record = payload as Record<string, unknown>;
    const message = record['message'] ?? record['title'];
    if (typeof message === 'string' && message.length > 0) {
      return message;
    }
  }

  if (error.message) {
    return error.message;
  }

  return fallback;
};

const normalizeValidationError = (error: HttpErrorResponse): NormalizedValidationError => {
  const payload = error.error;
  const fieldErrors: Record<string, string[]> = {};

  if (payload && typeof payload === 'object') {
    const record = payload as Record<string, unknown>;
    mergeFieldErrors(fieldErrors, record['errors']);
    mergeFieldErrors(fieldErrors, record['fieldErrors']);

    const notifications = record['notifications'];
    if (Array.isArray(notifications)) {
      for (const item of notifications) {
        if (!item || typeof item !== 'object') {
          continue;
        }
        const entry = item as Record<string, unknown>;
        const field = entry['field'] ?? entry['fieldName'] ?? entry['propertyName'] ?? entry['key'];
        const message = entry['message'] ?? entry['description'] ?? entry['error'];
        if (typeof field === 'string' && typeof message === 'string' && message.length > 0) {
          fieldErrors[field] = [...(fieldErrors[field] ?? []), message];
        }
      }
    }
  }

  const message = resolveMessage(error, payload, 'Validation failed. Check your input.');
  const code = payload && typeof payload === 'object' && !Array.isArray(payload)
    ? (typeof (payload as Record<string, unknown>)['code'] === 'string'
      ? (payload as Record<string, unknown>)['code'] as string
      : undefined)
    : undefined;

  return {
    status: error.status,
    code,
    message,
    fieldErrors: Object.keys(fieldErrors).length ? fieldErrors : undefined,
  };
};

const createHttpError = (error: NormalizedHttpError): NormalizedHttpError => error;

export const errorHandlingInterceptor: HttpInterceptorFn = (req, next) => {
  if (!isApiRequest(req.url)) {
    return next(req);
  }

  const tokenService = inject(AuthTokenService);
  const currentUserStore = inject(CurrentUserStore);
  const activeOrganizationStore = inject(ActiveOrganizationStore);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: unknown) => {
      if (!(error instanceof HttpErrorResponse)) {
        return throwError(() => error);
      }

      if (error.status === 401) {
        // Session invalid or expired: clear state and redirect to login once.
        tokenService.clearToken();
        currentUserStore.clear();
        activeOrganizationStore.clear();

        if (!isPublicRoute(router.url) && !isNavigatingToLogin) {
          isNavigatingToLogin = true;
          void router.navigateByUrl('/login', { replaceUrl: true })
            .finally(() => {
              isNavigatingToLogin = false;
            });
        }

        return throwError(() => createHttpError({
          status: 401,
          code: 'UNAUTHORIZED',
          message: 'Session expired. Please sign in again.',
        }));
      }

      if (error.status === 403) {
        // Access denied: keep session and return a normalized error.
        return throwError(() => createHttpError({
          status: 403,
          code: 'ACCESS_DENIED',
          message: 'You do not have permission to access this resource.',
        }));
      }

      if (error.status === 400 || error.status === 422) {
        // Validation errors: normalize payload for form handling.
        return throwError(() => normalizeValidationError(error));
      }

      if (error.status >= 500 || error.status === 0) {
        // Generic errors: avoid leaking backend details.
        return throwError(() => createHttpError({
          status: error.status,
          message: 'Something went wrong. Please try again later.',
        }));
      }

      return throwError(() => error);
    })
  );
};
