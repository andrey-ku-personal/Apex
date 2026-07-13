import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavigationSection } from './models/navigation-section';
import { ObjectKeysPipe } from './pipes/object-keys.pipe';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-layout-sidebar',
  imports: [
    ObjectKeysPipe,
    CommonModule,
    RouterModule,
    MatIconModule
  ],
  templateUrl: './layout-sidebar.html',
  styleUrl: './layout-sidebar.scss',
})
export class LayoutSidebar {
  protected readonly navigationSections: NavigationSection = {
    'Портфель': [
      { label: 'Дашборд', route: '/dashboard', icon: 'dashboard' },
      { label: 'Облигации', route: '/bond', icon: 'account_balance' },
      { label: 'Акции', route: '/share', icon: 'monitoring' },
      { label: 'Депозиты', route: '/deposit', icon: 'savings' }
    ],
    'Аналитика': [
      { label: 'Отчеты', route: '/report', icon: 'analytics' },
      { label: 'Календары Выплат', route: '/paymentSchedule', icon: 'calendar_month' }
    ],
    'Настроки': [
      { label: 'Настройки', route: '/setting', icon: 'settings' }
    ],
  };

  protected get sectionNames(): string[] {
    return Object.keys(this.navigationSections);
  }
}
