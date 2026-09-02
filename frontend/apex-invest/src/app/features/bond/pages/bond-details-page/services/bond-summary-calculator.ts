import { Injectable } from '@angular/core';
import {
  addMonths,
  differenceInCalendarMonths,
  getDaysInMonth,
  isAfter,
  isBefore,
  setDate,
  startOfDay,
} from 'date-fns';
import { BondDetailsOperation } from '../models/bond-details-operations';
import { FrequencySteps } from '../../../../../shared/options/frequency.options';

@Injectable()
export class BondSummaryCalculator {
  calculatePositionCount(operations: BondDetailsOperation[]): number {
    return operations.reduce(
      (sum, operation) => sum + (operation.type === 'buy' ? operation.count : -operation.count),
      0,
    );
  }

  calculateTotalBuySum(operations: BondDetailsOperation[]): number {
    return operations
      .filter((operation) => operation.type === 'buy')
      .reduce((sum, operation) => sum + operation.price * operation.count, 0);
  }

  calculateOverpay(operations: BondDetailsOperation[], parPrice: number): number {
    return this.calculateTotalBuySum(operations) - this.calculateBoughtCount(operations) * parPrice;
  }

  calculateNextPaymentDate(day: number, frequency: string, maturity: unknown): Date | null {
    if (frequency === 'end') {
      return this.parseDate(maturity);
    }

    const step = FrequencySteps[frequency];
    if (!step || !day || day < 1) {
      return null;
    }

    const maturityDate = this.parseDate(maturity);
    const today = startOfDay(new Date());

    for (let offset = 0; offset <= step; offset++) {
      const month = addMonths(today, offset);

      if (maturityDate && differenceInCalendarMonths(month, maturityDate) % step !== 0) {
        continue;
      }

      const candidate = setDate(month, Math.min(day, getDaysInMonth(month)));

      if (isBefore(candidate, today)) {
        continue;
      }

      if (maturityDate && isAfter(candidate, maturityDate)) {
        continue;
      }

      return candidate;
    }

    return null;
  }

  private calculateBoughtCount(operations: BondDetailsOperation[]): number {
    return operations
      .filter((operation) => operation.type === 'buy')
      .reduce((sum, operation) => sum + operation.count, 0);
  }

  private parseDate(value: unknown): Date | null {
    if (!value) {
      return null;
    }

    const date = value instanceof Date ? value : new Date(String(value));
    return isNaN(date.getTime()) ? null : date;
  }
}