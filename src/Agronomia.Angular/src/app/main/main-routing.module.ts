import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticationComponent } from './authentication/authentication.component';
import { FaqComponent } from './faq/faq.component';
import { PasswordRecoveryComponent } from './password-recovery/password-recovery.component';
import { PasswordResetComponent } from './password-reset/password-reset.component';
import { PasswordVerificationComponent } from './password-verification/password-verification.component';
import { PrivacyComponent } from './privacy/privacy.component';
import { TermsComponent } from './terms/terms.component';
import { MainComponent } from './main.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { authGuard } from './auth.guard';
import { FarmersPageComponent } from './farmers-page/farmers-page.component';
import { ResellersPageComponent } from './resellers-page/resellers-page.component';
import { LogisticsPageComponent } from './logistics-page/logistics-page.component';
import { PricingPageComponent } from './pricing-page/pricing-page.component';
import { AboutPageComponent } from './about-page/about-page.component';
import { CareersPageComponent } from './careers-page/careers-page.component';
import { BlogPageComponent } from './blog-page/blog-page.component';
import { ProductListingComponent } from './marketplace/product-listing.component';
import { ProductDetailComponent } from './marketplace/product-detail.component';
import { MarketplaceComponent } from './marketplace/marketplace.component';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'marketplace'
      },
      {
        path: 'marketplace',
        component: MarketplaceComponent
      },
      {
        path: 'marketplace/products',
        component: ProductListingComponent
      },
      {
        path: 'marketplace/products/:id',
        component: ProductDetailComponent
      },
      {
        path: 'platform/farmers',
        component: FarmersPageComponent
      },
      {
        path: 'platform/resellers',
        component: ResellersPageComponent
      },
      {
        path: 'platform/logistics',
        component: LogisticsPageComponent
      },
      {
        path: 'platform/pricing',
        component: PricingPageComponent
      },
      {
        path: 'platform/about',
        component: AboutPageComponent
      },
      {
        path: 'platform/careers',
        component: CareersPageComponent
      },
      {
        path: 'platform/blog',
        component: BlogPageComponent
      },
      {
        path: 'authentication',
        component: AuthenticationComponent
      },
      {
        path: 'dashboard',
        component: DashboardComponent,
        canActivate: [authGuard]
      },
      {
        path: 'forgot-password',
        pathMatch: 'full',
        component: PasswordRecoveryComponent
      },
      {
        path: 'forgot-password/code',
        component: PasswordVerificationComponent
      },
      {
        path: 'forgot-password/new-password',
        component: PasswordResetComponent
      },
      {
        path: 'platform/privacy',
        component: PrivacyComponent
      },
      {
        path: 'platform/faq',
        component: FaqComponent
      },
      {
        path: 'platform/terms',
        component: TermsComponent
      },
      // Temporary redirects to keep legacy Portuguese URLs working
      {
        path: 'marketplace',
        redirectTo: 'platform/marketplace',
        pathMatch: 'full'
      },
      {
        path: 'platform',
        redirectTo: 'platform/marketplace',
        pathMatch: 'full'
      },
      {
        path: 'marketplace/agricultores',
        redirectTo: 'platform/farmers',
        pathMatch: 'full'
      },
      {
        path: 'marketplace/revendedores',
        redirectTo: 'platform/resellers',
        pathMatch: 'full'
      },
      {
        path: 'marketplace/logistica',
        redirectTo: 'platform/logistics',
        pathMatch: 'full'
      },
      {
        path: 'marketplace/farmers',
        redirectTo: 'platform/farmers',
        pathMatch: 'full'
      },
      {
        path: 'marketplace/resellers',
        redirectTo: 'platform/resellers',
        pathMatch: 'full'
      },
      {
        path: 'marketplace/logistics',
        redirectTo: 'platform/logistics',
        pathMatch: 'full'
      },
      {
        path: 'privacidade',
        redirectTo: 'platform/privacy',
        pathMatch: 'full'
      },
      {
        path: 'termos',
        redirectTo: 'platform/terms',
        pathMatch: 'full'
      },
      {
        path: 'privacy',
        redirectTo: 'platform/privacy',
        pathMatch: 'full'
      },
      {
        path: 'terms',
        redirectTo: 'platform/terms',
        pathMatch: 'full'
      },
      {
        path: 'pricing',
        redirectTo: 'platform/pricing',
        pathMatch: 'full'
      },
      {
        path: 'about',
        redirectTo: 'platform/about',
        pathMatch: 'full'
      },
      {
        path: 'careers',
        redirectTo: 'platform/careers',
        pathMatch: 'full'
      },
      {
        path: 'blog',
        redirectTo: 'platform/blog',
        pathMatch: 'full'
      },
      {
        path: 'faq',
        redirectTo: 'platform/faq',
        pathMatch: 'full'
      },
      {
        path: 'sobre',
        redirectTo: 'platform/about',
        pathMatch: 'full'
      },
      {
        path: 'carreiras',
        redirectTo: 'platform/careers',
        pathMatch: 'full'
      },
      {
        path: '**',
        redirectTo: 'platform/marketplace'
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MainRoutingModule {}
