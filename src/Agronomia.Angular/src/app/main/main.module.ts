import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AuthenticationComponent } from './authentication/authentication.component';
import { FaqComponent } from './faq/faq.component';
import { PasswordRecoveryComponent } from './password-recovery/password-recovery.component';
import { PasswordResetComponent } from './password-reset/password-reset.component';
import { PasswordVerificationComponent } from './password-verification/password-verification.component';
import { CarouselCardComponent } from './shared/carousel-card/carousel-card.component';
import { PrivacyComponent } from './privacy/privacy.component';
import { TermsComponent } from './terms/terms.component';
import { MainRoutingModule } from './main-routing.module';
import { MainComponent } from './main.component';
import { ReactiveFormsModule } from '@angular/forms';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MarketplaceComponent } from './marketplace/marketplace.component';
import { FooterComponent } from './shared/footer/footer.component';
import { FarmersPageComponent } from './farmers-page/farmers-page.component';
import { ResellersPageComponent } from './resellers-page/resellers-page.component';
import { LogisticsPageComponent } from './logistics-page/logistics-page.component';
import { PricingPageComponent } from './pricing-page/pricing-page.component';
import { AboutPageComponent } from './about-page/about-page.component';
import { CareersPageComponent } from './careers-page/careers-page.component';
import { BlogPageComponent } from './blog-page/blog-page.component';

@NgModule({
  declarations: [
    MainComponent,
    AuthenticationComponent,
    PasswordRecoveryComponent,
    PasswordVerificationComponent,
    PasswordResetComponent,
    PrivacyComponent,
    TermsComponent,
    FaqComponent,
    DashboardComponent,
    MarketplaceComponent,
    FooterComponent,
    FarmersPageComponent,
    ResellersPageComponent,
    LogisticsPageComponent,
    PricingPageComponent,
    AboutPageComponent,
    CareersPageComponent,
    BlogPageComponent
  ],
  imports: [CommonModule, ReactiveFormsModule, MainRoutingModule, CarouselCardComponent]
})
export class MainModule {}
