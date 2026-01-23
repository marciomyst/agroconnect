import { Routes } from '@angular/router';
import { AppLayoutComponent } from '../layout/app-layout.component';
import { SellerCatalogFormPageComponent } from './catalog/pages/seller-catalog-form-page.component';
import { SellerCatalogPageComponent } from './catalog/pages/seller-catalog-page.component';

export const SELLER_ROUTES: Routes = [
  {
    path: '',
    component: AppLayoutComponent,
    children: [
      { path: 'catalog', component: SellerCatalogPageComponent },
      { path: 'catalog/new', component: SellerCatalogFormPageComponent },
      { path: 'catalog/:sellerProductId/edit', component: SellerCatalogFormPageComponent },
    ],
  },
];
