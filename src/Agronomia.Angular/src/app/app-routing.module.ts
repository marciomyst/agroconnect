import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./main/main.module').then((m) => m.MainModule)
  },
  {
    path: 'register',
    loadChildren: () => import('./registration/registration.module').then((m) => m.RegistrationModule)
  },
  {
    path: 'sellers',
    loadChildren: () => import('./seller/seller.module').then((m) => m.SellerModule)
  },
  {
    path: '**',
    redirectTo: ''
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
