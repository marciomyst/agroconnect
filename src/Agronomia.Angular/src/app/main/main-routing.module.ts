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

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'authentication'
      },
      {
        path: 'authentication',
        component: AuthenticationComponent
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
        path: 'privacidade',
        component: PrivacyComponent
      },
      {
        path: 'faq',
        component: FaqComponent
      },
      {
        path: 'termos',
        component: TermsComponent
      },
      {
        path: '**',
        redirectTo: 'authentication'
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MainRoutingModule {}
