import { Component, forwardRef, input } from '@angular/core';
import { NG_VALUE_ACCESSOR, ReactiveFormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormControlAbstract } from '../abstract/form-control.abstract';

@Component({
  selector: 'app-form-checkbox',
  imports: [ReactiveFormsModule, MatCheckboxModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormCheckbox),
      multi: true,
    },
  ],
  templateUrl: './form-checkbox.html',
  styleUrl: './form-checkbox.scss',
})
export class FormCheckbox extends FormControlAbstract<boolean> {
  public readonly label = input<string>('');
  public readonly text = input<string>('');
  public readonly note = input<string>('');
}