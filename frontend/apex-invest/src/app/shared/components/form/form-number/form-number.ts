import { Component, forwardRef, input } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormControlAbstract } from '../abstract/form-control.abstract';

@Component({
  selector: 'app-form-number',
  imports: [MatFormFieldModule, MatInputModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormNumber),
      multi: true,
    },
  ],
  templateUrl: './form-number.html',
  styleUrl: './form-number.scss',
})
export class FormNumber extends FormControlAbstract<number | null> {
  public readonly label = input<string>('');
  public readonly placeholder = input<string>('');
  public readonly isRequired = input<boolean>(false);
  public readonly step = input<number | string>('any');

  protected onInput(event: Event): void {
    const target = event.target as HTMLInputElement;
    const parsed = target.value === '' ? null : parseFloat(target.value);
    this.value = parsed;
    this.onChange(parsed);
    this.onTouch();
  }
}