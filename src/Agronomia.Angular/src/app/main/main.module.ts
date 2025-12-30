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

@NgModule({
  declarations: [
    MainComponent,
    AuthenticationComponent,
    PasswordRecoveryComponent,
    PasswordVerificationComponent,
    PasswordResetComponent,
    PrivacyComponent,
    TermsComponent,
    FaqComponent
  ],
  imports: [CommonModule, MainRoutingModule, CarouselCardComponent]
})
export class MainModule {}
