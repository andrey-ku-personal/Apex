import { Routes } from '@angular/router';
import { DashboardPage } from './features/dashboard/pages/dashboard-page/dashboard-page';
import { BondPage } from './features/bond/pages/bond-page/bond-page';
import { BondDetailsPage } from './features/bond/pages/bond-details-page/bond-details-page';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardPage },
  { path: 'bond', component: BondPage },
  { path: 'bond/:id', component: BondDetailsPage },
];