import { Component, computed, inject, input, output } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { BondListItem } from '../../models/bond-list-item';
import { BondTableModel } from '../../models/bond-table.model';
import { BondSummaryCalculator } from '../../../bond-details-page/services/bond-summary-calculator';
import { BondStatuses } from '../../../../../../shared/options/bond-status.options';
import { Currencies } from '../../../../../../shared/options/currency.options';

const FallbackStatusColor = '#475569';

@Component({
  selector: 'app-bond-list-table',
  imports: [DecimalPipe, MatButtonModule, MatIconModule, MatProgressSpinnerModule, MatTableModule],
  providers: [BondSummaryCalculator],
  templateUrl: './bond-list-table.html',
  styleUrl: './bond-list-table.scss',
})
export class BondListTable {
  private readonly calculator = inject(BondSummaryCalculator);

  protected readonly displayedColumns = [
    'ticker',
    'issuer',
    'rate',
    'price',
    'quantity',
    'sum',
    'status',
  ];

  readonly items = input.required<BondListItem[]>();
  readonly loading = input(false);
  readonly pageNumber = input.required<number>();
  readonly pageSize = input.required<number>();
  readonly totalCount = input.required<number>();

  readonly bondSelect = output<number>();
  readonly pageChange = output<number>();

  protected readonly rows = computed<BondTableModel[]>(() =>
    this.items().map((item) => this.createRow(item)),
  );

  protected readonly pageCount = computed(() =>
    Math.max(1, Math.ceil(this.totalCount() / Math.max(1, this.pageSize()))),
  );

  protected statusLabel(status: string): string {
    return this.findStatus(status)?.label ?? status ?? '—';
  }

  protected statusColor(status: string): string {
    return this.findStatus(status)?.color ?? FallbackStatusColor;
  }

  protected goToPage(page: number): void {
    const clamped = Math.min(Math.max(page, 1), this.pageCount());
    if (clamped !== this.pageNumber()) {
      this.pageChange.emit(clamped);
    }
  }

  private findStatus(status: string | undefined) {
    const key = status?.toLowerCase();
    return BondStatuses.find((option) => option.value === key);
  }

  private createRow(item: BondListItem): BondTableModel {
    const operations = item.operations ?? [];
    const quantity = this.calculator.calculatePositionCount(operations);
    const price = this.calculator.calculateLastBuyPrice(operations);

    return {
      id: item.id,
      ticker: item.ticker,
      issuer: item.issuer,
      couponRate: item.couponRate,
      currencyLabel: this.currencyLabel(item.currency),
      quantity,
      price,
      sum: quantity * price,
      status: item.status,
    };
  }

  private currencyLabel(currency: string): string {
    const key = currency?.toLowerCase();
    const option = Currencies.find((item) => String(item.value).toLowerCase() === key);
    return option?.label ?? currency;
  }
}
