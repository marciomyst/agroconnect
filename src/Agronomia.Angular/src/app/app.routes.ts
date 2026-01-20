import { Routes } from '@angular/router';
import { authCanMatch } from './core/guards/auth-can-match.guard';
import { roleCanMatch } from './core/guards/role-can-match.guard';
import { hasActiveOrganizationCanMatch } from './core/organization/guards/has-active-organization.can-match';

export const APP_ROUTES: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('./public/public.routes')
        .then(m => m.PUBLIC_ROUTES),
  },
  {
    path: 'farmer',
    canMatch: [authCanMatch, hasActiveOrganizationCanMatch, roleCanMatch('Farmer')],
    loadChildren: () =>
      import('./farmer/farmer.routes')
        .then(m => m.FARMER_ROUTES),
  },
  {
    path: 'seller',
    canMatch: [authCanMatch, hasActiveOrganizationCanMatch, roleCanMatch('Seller')],
    loadChildren: () =>
      import('./seller/seller.routes')
        .then(m => m.SELLER_ROUTES),
  },
  {
    path: 'admin',
    canMatch: [authCanMatch, hasActiveOrganizationCanMatch, roleCanMatch('Admin')],
    loadChildren: () =>
      import('./admin/admin.routes')
        .then(m => m.ADMIN_ROUTES),
  },
  { path: '**', redirectTo: '' },
];
