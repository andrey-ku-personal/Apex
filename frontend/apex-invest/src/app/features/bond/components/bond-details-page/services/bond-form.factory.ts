import { Injectable, inject } from '@angular/core';
import { NonNullableFormBuilder, Validators } from '@angular/forms';
import { BondDetails } from '../models/bond-details.model';
import { BondDetailsOperation } from '../models/bond-details-operations';
import { BondFormGroup } from '../form-groups/bond-form-group';
import { OperationFormGroup } from '../form-groups/operation-form-group';

@Injectable({ providedIn: 'root' })
export class BondFormFactory {
  private readonly fb = inject(NonNullableFormBuilder);

  createBondForm(bond?: Partial<BondDetails>): BondFormGroup {
    return this.fb.group({
      id: [bond?.id ?? 0],
      platformId: [bond?.platformId ?? 1],
      ticker: [bond?.ticker ?? '', [Validators.required]],
      issuer: [bond?.issuer ?? '', [Validators.required]],
      currency: [bond?.currency ?? 'BYN', [Validators.required]],
      parPrice: [bond?.parPrice ?? 0, [Validators.required, Validators.min(0)]],
      couponRate: [bond?.couponRate ?? 0, [Validators.required, Validators.min(0)]],
      paymentFrequency: [bond?.paymentFrequency ?? '', [Validators.required]],
      nextCouponDate: [bond?.nextCouponDate ?? 0],
      maturityDate: [bond?.maturityDate ?? '', [Validators.required]],
      status: [bond?.status ?? 'active', [Validators.required]],
      operations: this.fb.array(
        (bond?.operations ?? []).map((op) => this.createOperationGroup(op)),
      ),
    });
  }

  createOperationGroup(op?: Partial<BondDetailsOperation>): OperationFormGroup {
    return this.fb.group({
      type: [op?.type ?? ''],
      date: [op?.date ?? ''],
      price: [op?.price ?? 0],
      count: [op?.count ?? 0],
    });
  }
}