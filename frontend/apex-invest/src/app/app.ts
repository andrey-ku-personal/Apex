import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LayoutSidebar } from './features/layout/layout-sidebar/layout-sidebar';

@Component({
  selector: 'app-root',
  imports: [LayoutSidebar, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
}
