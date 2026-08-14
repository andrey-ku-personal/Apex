import { FormControl, FormGroup } from '@angular/forms';

export type OperationFormGroup = FormGroup<{
  type: FormControl<string>;
  date: FormControl<string>;
  price: FormControl<number>;
  count: FormControl<number>;
}>;