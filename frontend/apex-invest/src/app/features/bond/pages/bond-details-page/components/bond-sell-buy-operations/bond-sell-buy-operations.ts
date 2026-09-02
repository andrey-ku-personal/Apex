import { Component, inject, signal } from '@angular/core';
import { DatePipe, DecimalPipe } from '@angular/common';
import {
  FormGroup,
  FormArray,
  ControlContainer,
  FormGroupDirective,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Card } from '../../../../../../shared/components/card/card';
import { FormSelect } from '../../../../../../shared/components/form/form-select/form-select';
import { FormDate } from '../../../../../../shared/components/form/form-date/form-date';
import { FormNumber } from '../../../../../../shared/components/form/form-number/form-number';
import { OperationTypes } from '../../../../../../shared/options/operation-type.options';
import { FormTextArea } from '../../../../../../shared/components/form/form-text-area/form-text-area';
import { PlatformTypes } from '../../../../../../shared/options/platform.options';
import { pluralize } from '../../../../../../shared/utils/pluralize';
import { BondFormFactory } from '../../services/bond-form.factory';

@Component({
  selector: 'app-bond-sell-buy-operations',
  viewProviders: [
    {
      provide: ControlContainer,
      useExisting: FormGroupDirective,
    },
  ],
  imports: [
    DatePipe,
    DecimalPipe,
    ReactiveFormsModule,
    Card,
    MatIconModule,
    MatButtonModule,
    MatTableModule,
    MatInputModule,
    MatFormFieldModule,
    MatMenuModule,
    FormSelect,
    FormNumber,
    FormDate,
    FormTextArea,
  ],
  templateUrl: './bond-sell-buy-operations.html',
  styleUrl: './bond-sell-buy-operations.scss',
})
export class BondSellBuyOperations {
  private readonly parentForm = inject(FormGroupDirective);
  private readonly formFactory = inject(BondFormFactory);

  protected readonly form: FormGroup = this.formFactory.createOperationGroup();

  protected readonly operationTypes = OperationTypes;
  protected readonly platformTypes = PlatformTypes;
  protected readonly dataSource = new MatTableDataSource<FormGroup>([]);
  protected readonly selectedIndex = signal<number | null>(null);

  protected get operations(): FormArray {
    return this.parentForm.control.get('operations') as FormArray;
  }

  constructor() {
    this.parentForm.control.valueChanges.subscribe(() => this.syncDataSource());
  }

  protected operationsCountLabel(count: number): string {
    return pluralize(count, ['операция', 'операции', 'операций']);
  }

  protected onUpdateOperation(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();

    (this.operations.at(this.selectedIndex()!) as FormGroup).patchValue(value);

    this.resetForm();
  }

  protected onAddOperation(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const row = this.formFactory.createOperationGroup(this.form.getRawValue());

    this.operations.push(row);

    this.resetForm();
  }

  protected editOperation(index: number): void {
    const group = this.operations.at(index) as FormGroup;
    this.form.patchValue(group.getRawValue());
    this.selectedIndex.set(index);
  }

  protected removeOperation(index: number): void {
    this.operations.removeAt(index);

    const selected = this.selectedIndex();
    if (selected !== null) {
      if (selected === index) {
        this.resetForm();
      } else if (selected > index) {
        this.selectedIndex.set(selected - 1);
      }
    }
  }

  protected cancelEdit(): void {
    this.resetForm();
  }

  private resetForm(): void {
    this.form.reset();
    this.selectedIndex.set(null);
  }

  private syncDataSource(): void {
    this.dataSource.data = [...this.operations.controls] as FormGroup[];
  }
}