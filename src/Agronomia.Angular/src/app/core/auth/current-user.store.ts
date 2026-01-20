import { HttpErrorResponse } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { catchError, Observable, of, tap } from 'rxjs';
import { AuthApiService } from './auth-api.service';
import { AuthTokenService } from './auth-token.service';
import { CurrentUserContext, OrganizationType } from './auth.types';

@Injectable({ providedIn: 'root' })
export class CurrentUserStore {
  private readonly authApi = inject(AuthApiService);
  private readonly tokenService = inject(AuthTokenService);

  private readonly _user = signal<CurrentUserContext | null>(null);
  private readonly _loaded = signal(false);

  readonly user = this._user.asReadonly();
  readonly isLoaded = this._loaded.asReadonly();
  readonly isAuthenticated = computed(() => this._user() !== null);
  readonly organizations = computed(() => this._user()?.organizations ?? []);
  readonly roles = computed(() => {
    const roleSet = new Set<string>();
    for (const organization of this.organizations()) {
      for (const role of organization.roles) {
        roleSet.add(role);
      }
    }
    return Array.from(roleSet);
  });

  setUser(context: CurrentUserContext | null): void {
    this._user.set(context);
    this._loaded.set(true);
  }

  clear(): void {
    this._user.set(null);
    this._loaded.set(true);
  }

  loadFromApi(): Observable<CurrentUserContext | null> {
    return this.authApi.getCurrentUserContext().pipe(
      tap(context => this.setUser(context)),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          this.tokenService.clearToken();
        }
        this.clear();
        return of(null);
      })
    );
  }

  hasRole(role: string): boolean {
    return this.roles().includes(role);
  }

  hasOrganizationType(type: OrganizationType): boolean {
    return this.organizations().some(org => org.type === type);
  }
}
