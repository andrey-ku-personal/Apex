import { Component, computed, effect, inject, input, signal } from '@angular/core';
import { MatDividerModule } from '@angular/material/divider';
import { format } from 'date-fns';
import { ru } from 'date-fns/locale';
import { Card } from '../../../../../../shared/components/card/card';
import { BondFormGroup } from '../../form-groups/bond-form-group';
import { BondSummaryCalculator } from '../../services/bond-summary-calculator';

@Component({
  selector: 'app-bond-summary',
  imports: [Card, MatDividerModule],
  providers: [BondSummaryCalculator],
  templateUrl: './bond-summary.html',
  styleUrl: './bond-summary.scss',
})
export class BondSummary {
  private readonly calculator = inject(BondSummaryCalculator);

  readonly form = input.required<BondFormGroup>();

  protected readonly formValue = signal<ReturnType<BondFormGroup['getRawValue']> | null>(null);

  protected readonly positionCount = computed(() =>
    this.calculator.calculatePositionCount(this.formValue()?.operations ?? []),
  );

  protected readonly totalBuySum = computed(() =>
    this.calculator.calculateTotalBuySum(this.formValue()?.operations ?? []),
  );

  protected readonly overpay = computed(() => {
    const value = this.formValue();
    return value ? this.calculator.calculateOverpay(value.operations, value.parPrice) : null;
  });

  protected readonly nextPaymentLabel = computed(() => {
    const value = this.formValue();
    if (!value) {
      return '—';
    }

    const date = this.calculator.calculateNextPaymentDate(
      value.nextCouponDate,
      value.paymentFrequency,
      value.maturityDate,
    );

    return date ? format(date, 'dd.MM.yyyy') : '—';
  });

  constructor() {
    effect((onCleanup) => {
      const form = this.form();
      this.formValue.set(form.getRawValue());

      const sub = form.valueChanges.subscribe(() => this.formValue.set(form.getRawValue()));
      onCleanup(() => sub.unsubscribe());
    });
  }
}