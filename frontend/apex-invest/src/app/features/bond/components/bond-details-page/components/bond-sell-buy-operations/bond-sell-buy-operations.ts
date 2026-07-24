import { Component, computed, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  FormArray,
  ControlContainer,
  FormGroupDirective,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormSelect } from '../../../../../../shared/components/form/form-select/form-select';
import { FormDate } from '../../../../../../shared/components/form/form-date/form-date';
import { FormNumber } from '../../../../../../shared/components/form/form-number/form-number';
import { OperationTypes } from '../../../../../../shared/options/operation-type.options';

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
    ReactiveFormsModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatTableModule,
    MatInputModule,
    MatFormFieldModule,
    FormSelect,
    FormNumber,
    FormDate,
  ],
  templateUrl: './bond-sell-buy-operations.html',
  styleUrl: './bond-sell-buy-operations.scss',
})
export class BondSellBuyOperations {
  private readonly parentForm = inject(FormGroupDirective);
  private readonly fb = inject(FormBuilder);

  protected readonly form: FormGroup = this.fb.group({
    type: ['buy', [Validators.required]],
    date: [null, [Validators.required]],
    price: [null, [Validators.required, Validators.min(0)]],
    count: [null, [Validators.required, Validators.min(1)]],
  });
  protected readonly operationTypes = OperationTypes;
  protected readonly displayedColumns = ['type', 'date', 'price', 'count', 'sum', 'actions'];
  protected readonly dataSource = new MatTableDataSource<FormGroup>([]);
  protected readonly selectedIndex = signal<number | null>(null);

  protected operations = computed(() => this.parentForm.control.get('operations') as FormArray);  

  protected addOperation(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();

    if (this.selectedIndex()) {
      (this.operations().at(this.selectedIndex()!) as FormGroup).patchValue(value);
    } else {
      this.operations().push(this.fb.group(value));
    }

    this.syncDataSource();
    this.resetForm();
  }

  protected editOperation(index: number): void {
    const group = this.operations().at(index) as FormGroup;
    this.form.patchValue(group.getRawValue());
    this.selectedIndex.set(index);
  }

  protected removeOperation(index: number): void {
    this.operations().removeAt(index);

    if (this.selectedIndex() === index) {
      this.resetForm();
    }

    this.syncDataSource();
  }

  protected cancelEdit(): void {
    this.resetForm();
  }

  private resetForm(): void {
    this.form.reset({ type: 'buy' });
    this.selectedIndex.set(null);
  }

  private syncDataSource(): void {
    this.dataSource.data = [...this.operations().controls] as FormGroup[];
  }
}