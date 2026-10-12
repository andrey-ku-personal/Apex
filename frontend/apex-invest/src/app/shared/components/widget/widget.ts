import { Component, input, TemplateRef } from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';

import { Card } from '../card/card';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-widget',
  imports: [Card, NgTemplateOutlet, MatIcon],
  host: {
    '[style.height]': 'height()',
  },
  templateUrl: './widget.html',
  styleUrl: './widget.scss',
})
export class Widget {
  public readonly icon = input<string>('');
  public readonly title = input.required<string>();
  public readonly titleLeft = input<TemplateRef<unknown> | null>(null);
  public readonly empty = input(false);
  public readonly emptyTemplate = input<TemplateRef<unknown> | null>(null);
  public readonly height = input('380px');
}