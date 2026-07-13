import { Routes } from '@angular/router';
import { DashboardPage } from './features/dashboard/components/dashboard-page/dashboard-page';
import { SharePage } from './features/share/components/share-page/share-page';
import { DepositPage } from './features/deposit/components/deposit-page/deposit-page';
import { BondPage } from './features/bond/components/bond-page/bond-page';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardPage },
  { path: 'share', component: SharePage },
  { path: 'bond', component: BondPage },
  { path: 'deposit', component: DepositPage }
];
