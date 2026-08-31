import { Component, input } from '@angular/core';
import { MatCard, MatCardContent, MatCardHeader } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-card',
  imports: [MatCard, MatCardHeader, MatCardContent, MatIcon],
  templateUrl: './card.html',
  styleUrl: './card.scss',
})
export class Card {
  public readonly icon = input<string>('');
  public readonly title = input.required<string>();
}