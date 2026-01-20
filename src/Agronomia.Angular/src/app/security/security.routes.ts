import { Routes } from '@angular/router';
import { AppLayoutComponent } from '../layout/app-layout.component';
import { ChangePasswordComponent } from './change-password.component';

export const SECURITY_ROUTES: Routes = [
  {
    path: '',
    component: AppLayoutComponent,
    children: [
      {
        path: '',
        component: ChangePasswordComponent,
      },
    ],
  },
];
