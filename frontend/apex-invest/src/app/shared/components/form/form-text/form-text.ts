import { Component, forwardRef, input } from '@angular/core';
import { NG_VALUE_ACCESSOR, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormControlAbstract } from '../abstract/form-control.abstract';

@Component({
  selector: 'app-form-text',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormText),
      multi: true,
    },
  ],
  templateUrl: './form-text.html',
  styleUrl: './form-text.scss',
})
export class FormText extends FormControlAbstract<string> {
  public readonly label = input<string>('');
  public readonly placeholder = input<string>('');
  public readonly isRequired = input<boolean>(false);
}