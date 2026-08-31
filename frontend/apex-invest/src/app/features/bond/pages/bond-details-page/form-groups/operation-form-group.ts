import { FormControl, FormGroup } from '@angular/forms';

export type OperationFormGroup = FormGroup<{
  type: FormControl<string>;
  platformId: FormControl<number>;
  date: FormControl<string>;
  price: FormControl<number>;
  count: FormControl<number>;
  comment: FormControl<string>;
}>;