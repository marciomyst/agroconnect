import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CurrentUserOrganization } from '../../core/auth/auth.types';
import { CurrentUserStore } from '../../core/auth/current-user.store';
import { ActiveOrganizationStore } from '../../core/organization/active-organization.store';

@Component({
  selector: 'app-organization-select',
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="organization-select">
      <h1>Select organization</h1>
      <p *ngIf="organizations().length === 0">
        No organizations available for this user.
      </p>
      <ul *ngIf="organizations().length">
        <li *ngFor="let organization of organizations()">
          <button type="button" (click)="selectOrganization(organization)">
            {{ organization.organizationName }} ({{ organization.type }})
          </button>
        </li>
      </ul>
    </section>
  `,
})
export class OrganizationSelectComponent {
  private readonly currentUserStore = inject(CurrentUserStore);
  private readonly activeOrganizationStore = inject(ActiveOrganizationStore);
  private readonly router = inject(Router);

  readonly organizations = this.currentUserStore.organizations;

  selectOrganization(organization: CurrentUserOrganization): void {
    this.activeOrganizationStore.setActiveOrganization({
      organizationId: organization.organizationId,
      organizationName: organization.organizationName,
      organizationType: organization.type,
      roles: organization.roles,
    });

    void this.router.navigateByUrl('/');
  }
}
