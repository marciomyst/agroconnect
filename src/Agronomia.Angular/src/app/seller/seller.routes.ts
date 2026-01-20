import { Routes } from '@angular/router';
import { AppLayoutComponent } from '../layout/app-layout.component';

export const SELLER_ROUTES: Routes = [
  {
    path: '',
    component: AppLayoutComponent,
    children: [],
  },
];
