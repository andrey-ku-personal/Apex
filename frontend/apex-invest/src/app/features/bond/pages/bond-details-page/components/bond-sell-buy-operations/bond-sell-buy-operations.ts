import { Component, inject } from '@angular/core';
import { DatePipe, DecimalPipe } from '@angular/common';
import { AbstractControl, FormArray, FormGroup, FormGroupDirective } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatMenuModule } from '@angular/material/menu';
import { Widget } from '../../../../../../shared/components/widget/widget';
import { pluralize } from '../../../../../../shared/utils/pluralize';
import { BondFormFactory } from '../../services/bond-form.factory';
import { BondDetailsOperation } from '../../models/bond-details-operations';
import { BondOperationDialog, BondOperationDialogData } from '../bond-operation-dialog/bond-operation-dialog';

@Component({
  selector: 'app-bond-sell-buy-operations',
  imports: [
    DatePipe,
    DecimalPipe,
    Widget,
    MatIconModule,
    MatButtonModule,
    MatMenuModule,
  ],
  templateUrl: './bond-sell-buy-operations.html',
  styleUrl: './bond-sell-buy-operations.scss',
})
export class BondSellBuyOperations {
  private readonly parentForm = inject(FormGroupDirective);
  private readonly formFactory = inject(BondFormFactory);
  private readonly dialog = inject(MatDialog);

  protected get operations(): FormArray {
    return this.parentForm.control.get('operations') as FormArray;
  }

  protected operationType(group: AbstractControl): string {
    return String(group.get('type')?.value ?? '').toLowerCase();
  }

  protected operationSum(group: AbstractControl): number {
    const price = Number(group.get('price')?.value) || 0;
    const count = Number(group.get('count')?.value) || 0;

    return price * count;
  }

  protected operationsCountLabel(count: number): string {
    return pluralize(count, ['операция', 'операции', 'операций']);
  }

  protected openCreateDialog(): void {
    this.dialog
      .open<BondOperationDialog, BondOperationDialogData, BondDetailsOperation>(
        BondOperationDialog,
        {
          width: 'min(960px, 96vw)',
          maxWidth: '96vw',
          minHeight: '640px',
          maxHeight: '92vh',
          data: {},
        },
      )
      .afterClosed()
      .subscribe((result) => {
        if (!result) {
          return;
        }

        this.operations.push(this.formFactory.createOperationGroup(result));
      });
  }

  protected openEditDialog(index: number): void {
    const group = this.operations.at(index) as FormGroup;

    this.dialog
      .open<BondOperationDialog, BondOperationDialogData, BondDetailsOperation>(
        BondOperationDialog,
        {
          width: 'min(960px, 96vw)',
          maxWidth: '96vw',
          minHeight: '640px',
          maxHeight: '92vh',
          data: { operation: group.getRawValue() },
        },
      )
      .afterClosed()
      .subscribe((result) => {
        if (!result) {
          return;
        }

        group.patchValue(result);
      });
  }

  protected removeOperation(index: number): void {
    this.operations.removeAt(index);
  }
}