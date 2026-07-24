import { Component, forwardRef, input } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { FormControlAbstract } from '../abstract/form-control.abstract';
import { OptionModel } from '../../../models/option.model';

@Component({
  selector: 'app-form-select',
  imports: [MatFormFieldModule, MatSelectModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormSelect),
      multi: true,
    },
  ],
  templateUrl: './form-select.html',
  styleUrl: './form-select.scss',
})
export class FormSelect extends FormControlAbstract<string> {
  public readonly label = input<string>('');
  public readonly placeholder = input<string>('');
  public readonly isRequired = input<boolean>(false);
  public readonly options = input<OptionModel[]>([]);

  protected onSelect(value: string): void {
    this.value = value;
    this.onChange(value);
    this.onTouch();
  }
}
