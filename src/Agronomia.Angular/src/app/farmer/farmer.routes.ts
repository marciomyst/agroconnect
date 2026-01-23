import { Routes } from '@angular/router';
import { AppLayoutComponent } from '../layout/app-layout.component';
import { FarmerPurchaseIntentListPageComponent } from './purchase-intents/farmer-purchase-intent-list-page.component';
import { PurchaseIntentCreatePageComponent } from './purchase-intents/purchase-intent-create-page.component';
import { MarketplaceListPageComponent } from '../marketplace/list/marketplace-list-page.component';
import { MarketplaceProductDetailPageComponent } from '../marketplace/detail/marketplace-product-detail-page.component';

export const FARMER_ROUTES: Routes = [
  {
    path: '',
    component: AppLayoutComponent,
    children: [
      { path: 'marketplace', component: MarketplaceListPageComponent },
      { path: 'marketplace/products/:productId', component: MarketplaceProductDetailPageComponent },
      { path: 'marketplace/products/:productId/intent', component: PurchaseIntentCreatePageComponent },
      { path: 'purchase-intents', component: FarmerPurchaseIntentListPageComponent },
    ],
  },
];
