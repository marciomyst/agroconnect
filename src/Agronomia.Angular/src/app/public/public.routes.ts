import { Routes } from '@angular/router';
import { authCanMatch } from '../core/guards/auth-can-match.guard';
import { PublicLayoutComponent } from '../layout/public-layout.component';
import { PublicLandingComponent } from './landing/landing.component';
import { PublicLoginComponent } from './login/login.component';
import { OrganizationSelectComponent } from './organization-select/organization-select.component';

export const PUBLIC_ROUTES: Routes = [
  {
    path: '',
    component: PublicLayoutComponent,
    children: [
      { path: '', pathMatch: 'full', component: PublicLandingComponent },
      { path: 'login', component: PublicLoginComponent },
      {
        path: 'select-organization',
        canMatch: [authCanMatch],
        component: OrganizationSelectComponent,
      },
    ],
  },
];
