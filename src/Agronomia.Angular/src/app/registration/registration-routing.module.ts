import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegistrationSelectionComponent } from './registration-selection.component';

const routes: Routes = [
  {
    path: '',
    component: RegistrationSelectionComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistrationRoutingModule {}
