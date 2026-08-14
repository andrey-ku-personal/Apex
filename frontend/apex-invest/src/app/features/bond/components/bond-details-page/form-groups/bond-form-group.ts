import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { OperationFormGroup } from './operation-form-group';

export type BondFormGroup = FormGroup<{
  id: FormControl<number>;
  platformId: FormControl<number>;
  ticker: FormControl<string>;
  issuer: FormControl<string>;
  currency: FormControl<string>;
  parPrice: FormControl<number>;
  couponRate: FormControl<number>;
  paymentFrequency: FormControl<string>;
  nextCouponDate: FormControl<number>;
  maturityDate: FormControl<string>;
  status: FormControl<string>;
  operations: FormArray<OperationFormGroup>;
}>;