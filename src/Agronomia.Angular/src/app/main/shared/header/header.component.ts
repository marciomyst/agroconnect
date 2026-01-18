import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { take } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from '../../authentication/auth.service';
import { UserProfileService } from './user-profile.service';

@Component({
  selector: 'app-site-header',
  templateUrl: './header.component.html',
  standalone: false,
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit, OnChanges {
  @Input() isAuthenticated: boolean | null = null;
  @Input() locationLabel = 'Entregar em:';
  @Input() locationValue = 'Fazenda Santa Clara';
  @Input() showLocation = true;
  @Input() userName = 'Usuario';
  @Input() userRole = 'Produtor';
  @Input() userAvatarUrl = '';
  @Input() userPendingOrders = 0;

  resolvedIsAuthenticated = false;
  isLoadingProfile = false;

  constructor(
    private readonly authService: AuthService,
    private readonly userProfileService: UserProfileService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.syncAuthState();
    if (this.resolvedIsAuthenticated) {
      void this.loadUserProfile();
    }
  }

  ngOnChanges(): void {
    this.syncAuthState();
    if (this.resolvedIsAuthenticated) {
      void this.loadUserProfile();
    }
  }

  goHome(): void {
    void this.router.navigate(['/marketplace']);
  }

  goToLogin(): void {
    void this.router.navigate(['/authentication']);
  }

  handlePrimaryCta(): void {
    if (this.resolvedIsAuthenticated) {
      void this.router.navigate(['/dashboard']);
      return;
    }

    this.goToLogin();
  }

  handleProfile(): void {
    void this.router.navigate(['/dashboard']);
  }

  handleOrders(): void {
    void this.router.navigate(['/dashboard']);
  }

  handlePrescriptions(): void {
    void this.router.navigate(['/dashboard']);
  }

  handleSettings(): void {
    void this.router.navigate(['/dashboard/settings']);
  }

  handleHelp(): void {
    void this.router.navigate(['/platform/faq']);
  }

  handleLogout(): void {
    this.authService.logout();
    this.userProfileService.clearCache();
    void this.router.navigate(['/authentication']);
  }

  private syncAuthState(): void {
    this.resolvedIsAuthenticated = this.isAuthenticated ?? this.authService.hasActiveSession();
  }

  private async loadUserProfile(): Promise<void> {
    if (this.isLoadingProfile) {
      return;
    }

    this.isLoadingProfile = true;

    this.userProfileService
      .getProfile()
      .pipe(take(1))
      .subscribe(profile => {
        if (profile) {
          this.userName = profile.name || this.userName;
          this.userRole = profile.role || this.userRole;
        }
        this.isLoadingProfile = false;
      });
  }
}
