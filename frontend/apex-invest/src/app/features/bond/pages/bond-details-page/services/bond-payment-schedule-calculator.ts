import { Injectable } from '@angular/core';
import {
  addMonths,
  differenceInCalendarDays,
  differenceInCalendarMonths,
  getDaysInMonth,
  isAfter,
  isBefore,
  setDate,
  startOfDay,
} from 'date-fns';
import { BondDetailsOperation } from '../models/bond-details-operations';
import { BondPayment } from '../models/bond-payment';
import { FrequencySteps } from '../../../../../shared/options/frequency.options';

@Injectable()
export class BondPaymentScheduleCalculator {
  calculatePaymentSchedule(input: {
    operations: BondDetailsOperation[];
    parPrice: number;
    couponRate: number;
    frequency: string;
    day: number;
    maturity: unknown;
  }): BondPayment[] {
    const maturityDate = this.parseDate(input.maturity);
    if (!maturityDate) {
      return [];
    }

    const firstBuyDate = this.calculateFirstBuyDate(input.operations);
    if (!firstBuyDate) {
      return [];
    }

    const today = startOfDay(new Date());

    if (input.frequency === 'end') {
      const count = this.calculatePositionAt(input.operations, maturityDate);
      if (count <= 0) {
        return [];
      }

      return [
        {
          number: 1,
          date: maturityDate,
          count,
          amount: count * input.parPrice * (input.couponRate / 100),
          received: isBefore(maturityDate, today),
        },
      ];
    }

    const step = FrequencySteps[input.frequency];
    if (!step || !input.day || input.day < 1 || !input.parPrice || !input.couponRate) {
      return [];
    }

    if (isAfter(firstBuyDate, maturityDate)) {
      return [];
    }

    const buyMonth = new Date(firstBuyDate.getFullYear(), firstBuyDate.getMonth(), 1);
    const maturityMonth = new Date(maturityDate.getFullYear(), maturityDate.getMonth(), 1);
    const buyOffset = differenceInCalendarMonths(buyMonth, maturityMonth);
    const startMonth = addMonths(buyMonth, ((-buyOffset % step) + step) % step);
    const monthsTotal = differenceInCalendarMonths(maturityMonth, startMonth);

    const payments: BondPayment[] = [];

    for (let months = 0; months <= monthsTotal; months += step) {
      const month = addMonths(startMonth, months);
      const date = this.resolvePaymentDate(month, input.day);

      if (isBefore(date, firstBuyDate) || isAfter(date, maturityDate)) {
        continue;
      }

      const count = this.calculatePositionAt(input.operations, date);
      if (count <= 0) {
        continue;
      }

      const previousDate = this.resolvePaymentDate(addMonths(month, -step), input.day);
      const days = differenceInCalendarDays(date, previousDate);

      payments.push({
        number: payments.length + 1,
        date,
        count,
        amount: this.roundAmount(count * input.parPrice * (input.couponRate / 100) * (days / 365)),
        received: isBefore(date, today),
      });
    }

    return payments;
  }

  private calculateFirstBuyDate(operations: BondDetailsOperation[]): Date | null {
    return (
      operations
        .filter((operation) => operation.type === 'buy')
        .map((operation) => this.parseDate(operation.date))
        .filter((date): date is Date => date !== null)
        .sort((a, b) => a.getTime() - b.getTime())[0] ?? null
    );
  }

  private calculatePositionAt(operations: BondDetailsOperation[], date: Date): number {
    return operations.reduce((sum, operation) => {
      const operationDate = this.parseDate(operation.date);
      if (!operationDate || !isBefore(operationDate, date)) {
        return sum;
      }
      return sum + (operation.type === 'buy' ? operation.count : -operation.count);
    }, 0);
  }

  private resolvePaymentDate(month: Date, day: number): Date {
    return setDate(month, Math.min(day, getDaysInMonth(month)));
  }

  private roundAmount(value: number): number {
    return Math.round(value * 100) / 100;
  }

  private parseDate(value: unknown): Date | null {
    if (!value) {
      return null;
    }

    const date = value instanceof Date ? value : new Date(String(value));
    return isNaN(date.getTime()) ? null : date;
  }
}