import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';

@Component({
  selector: 'app-bond-page',
  imports: [MatButtonModule, MatIconModule],
  templateUrl: './bond-page.html',
  styleUrl: './bond-page.scss',
})
export class BondPage {
  private readonly router = inject(Router);

  protected createBond(): void {
    this.router.navigate(['/bond', 0]);
  }
}