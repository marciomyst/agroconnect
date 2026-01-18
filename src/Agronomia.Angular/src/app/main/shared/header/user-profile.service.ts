import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, of, tap } from 'rxjs';
import { AuthService } from '../../authentication/auth.service';

export type UserProfile = {
  id: string;
  email: string;
  name: string;
  role: string;
  createdAt: string;
};

type CachedProfile = {
  data: UserProfile;
  expiresAt: number;
};

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {
  private readonly endpoint = '/api/users/me';
  private readonly cacheKey = 'agroconnect.user.me';
  private readonly ttlMs = 5 * 60 * 1000;

  private memoryCache: CachedProfile | null = null;

  constructor(
    private readonly http: HttpClient,
    private readonly authService: AuthService
  ) {}

  getProfile(forceRefresh = false): Observable<UserProfile | null> {
    if (!forceRefresh) {
      const cached = this.getCachedProfile();
      if (cached) {
        return of(cached);
      }
    }

    const headers = this.buildAuthHeaders();
    if (!headers) {
      return of(null);
    }

    return this.http.get<UserProfile>(this.endpoint, { headers }).pipe(
      tap(profile => this.setCachedProfile(profile)),
      catchError(() => of(null))
    );
  }

  clearCache(): void {
    this.memoryCache = null;
    localStorage.removeItem(this.cacheKey);
  }

  private getCachedProfile(): UserProfile | null {
    if (this.memoryCache && this.memoryCache.expiresAt > Date.now()) {
      return this.memoryCache.data;
    }

    const stored = localStorage.getItem(this.cacheKey);
    if (!stored) {
      return null;
    }

    try {
      const parsed = JSON.parse(stored) as CachedProfile;
      if (parsed.expiresAt > Date.now()) {
        this.memoryCache = parsed;
        return parsed.data;
      }
    } catch {
      return null;
    }

    return null;
  }

  private setCachedProfile(profile: UserProfile): void {
    const cached: CachedProfile = {
      data: profile,
      expiresAt: Date.now() + this.ttlMs
    };

    this.memoryCache = cached;
    localStorage.setItem(this.cacheKey, JSON.stringify(cached));
  }

  private buildAuthHeaders(): HttpHeaders | null {
    const token = this.authService.getAccessToken();
    if (!token) {
      return null;
    }

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }
}
