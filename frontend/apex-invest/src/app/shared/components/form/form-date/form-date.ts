import { Component, forwardRef, input } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatIconModule } from '@angular/material/icon';
import { FormControlAbstract } from '../abstract/form-control.abstract';

@Component({
  selector: 'app-form-date',
  imports: [MatFormFieldModule, MatInputModule, MatDatepickerModule, MatIconModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormDate),
      multi: true,
    },
  ],
  templateUrl: './form-date.html',
  styleUrl: './form-date.scss',
})
export class FormDate extends FormControlAbstract<Date | null> {
  public readonly label = input<string>('');
  public readonly placeholder = input<string>('');
  public readonly isRequired = input<boolean>(false);

  protected onSelect(value: Date | null): void {
    this.value = value;
    this.onChange(value);
    this.onTouch();
  }
}