import { Component, forwardRef, input } from '@angular/core';
import { FormControl, NG_VALUE_ACCESSOR, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatIconModule } from '@angular/material/icon';
import { format, isValid, parse, startOfDay } from 'date-fns';
import { FormControlAbstract } from '../abstract/form-control.abstract';

@Component({
  selector: 'app-form-date',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatDatepickerModule, MatIconModule],
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
export class FormDate extends FormControlAbstract<string> {
  public readonly label = input<string>('');
  public readonly placeholder = input<string>('');
  public readonly isRequired = input<boolean>(false);

  protected readonly internal = new FormControl<Date | null>(null);

  public override writeValue(value: string | null): void {
    this.internal.setValue(this.parseDate(value), { emitEvent: false });
  }

  public override setDisabledState(isDisabled: boolean): void {
    isDisabled ? this.internal.disable() : this.internal.enable();
  }

  protected onDateChange(): void {
    const date = this.internal.value;

    this.control.markAsTouched();
    this.onChange(date && isValid(date) ? format(date, 'yyyy-MM-dd') : '');
  }

  private parseDate(value: string | null | undefined): Date | null {
    if (!value) {
      return null;
    }

    const date = parse(value.slice(0, 10), 'yyyy-MM-dd', new Date());
    return isValid(date) ? startOfDay(date) : null;
  }
}