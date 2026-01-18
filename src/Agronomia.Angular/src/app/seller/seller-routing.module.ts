import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SellerOnboardingComponent } from '../main/marketplace/seller-onboarding.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'register',
    pathMatch: 'full'
  },
  {
    path: 'register',
    component: SellerOnboardingComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SellerRoutingModule {}
