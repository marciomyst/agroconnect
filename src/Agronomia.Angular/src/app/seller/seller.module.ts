import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { SellerOnboardingComponent } from '../main/marketplace/seller-onboarding.component';
import { SharedUiModule } from '../main/shared/shared-ui.module';
import { SellerRoutingModule } from './seller-routing.module';

@NgModule({
  declarations: [SellerOnboardingComponent],
  imports: [CommonModule, ReactiveFormsModule, SharedUiModule, SellerRoutingModule]
})
export class SellerModule {}
