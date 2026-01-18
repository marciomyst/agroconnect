import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { RegistrationSelectionComponent } from './registration-selection.component';
import { RegistrationRoutingModule } from './registration-routing.module';

@NgModule({
  declarations: [RegistrationSelectionComponent],
  imports: [CommonModule, RouterModule, RegistrationRoutingModule]
})
export class RegistrationModule {}
