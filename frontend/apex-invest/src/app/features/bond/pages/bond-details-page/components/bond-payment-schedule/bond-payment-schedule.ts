import { Component, computed, effect, inject, input, signal } from '@angular/core';
import { format } from 'date-fns';
import { Card } from '../../../../../../shared/components/card/card';
import { BondFormGroup } from '../../form-groups/bond-form-group';
import { BondPaymentScheduleCalculator } from '../../services/bond-payment-schedule-calculator';

@Component({
  selector: 'app-bond-payment-schedule',
  imports: [Card],
  providers: [BondPaymentScheduleCalculator],
  templateUrl: './bond-payment-schedule.html',
  styleUrl: './bond-payment-schedule.scss',
})
export class BondPaymentSchedule {
  private readonly calculator = inject(BondPaymentScheduleCalculator);

  readonly form = input.required<BondFormGroup>();

  protected readonly formValue = signal<ReturnType<BondFormGroup['getRawValue']> | null>(null);

  protected readonly payments = computed(() => {
    const value = this.formValue();
    if (!value) {
      return [];
    }

    return this.calculator.calculatePaymentSchedule({
      operations: value.operations,
      parPrice: value.parPrice,
      couponRate: value.couponRate,
      frequency: value.paymentFrequency,
      day: value.nextCouponDate,
      maturity: value.maturityDate,
    });
  });

  protected readonly receivedSum = computed(() =>
    this.payments()
      .filter((payment) => payment.received)
      .reduce((sum, payment) => sum + payment.amount, 0),
  );

  protected readonly upcomingSum = computed(() =>
    this.payments()
      .filter((payment) => !payment.received)
      .reduce((sum, payment) => sum + payment.amount, 0),
  );

  protected readonly hasPurchases = computed(() =>
    (this.formValue()?.operations ?? []).some((operation) => operation.type === 'buy'),
  );

  protected formatDate(date: Date): string {
    return format(date, 'dd.MM.yy');
  }

  protected formatAmount(amount: number): string {
    return amount.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  constructor() {
    effect((onCleanup) => {
      const form = this.form();
      this.formValue.set(form.getRawValue());

      const sub = form.valueChanges.subscribe(() => this.formValue.set(form.getRawValue()));
      onCleanup(() => sub.unsubscribe());
    });
  }
}