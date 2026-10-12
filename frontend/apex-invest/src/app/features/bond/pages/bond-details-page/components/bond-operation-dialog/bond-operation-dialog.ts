import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ReactiveFormsModule } from '@angular/forms';
import { FormSelect } from '../../../../../../shared/components/form/form-select/form-select';
import { FormDate } from '../../../../../../shared/components/form/form-date/form-date';
import { FormNumber } from '../../../../../../shared/components/form/form-number/form-number';
import { FormTextArea } from '../../../../../../shared/components/form/form-text-area/form-text-area';
import { OperationTypes } from '../../../../../../shared/options/operation-type.options';
import { PlatformTypes } from '../../../../../../shared/options/platform.options';
import { BondFormFactory } from '../../services/bond-form.factory';
import { OperationFormGroup } from '../../form-groups/operation-form-group';
import { BondDetailsOperation } from '../../models/bond-details-operations';

export interface BondOperationDialogData {
  operation?: Partial<BondDetailsOperation>;
}

@Component({
  selector: 'app-bond-operation-dialog',
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    FormSelect,
    FormNumber,
    FormDate,
    FormTextArea,
  ],
  templateUrl: './bond-operation-dialog.html',
  styleUrl: './bond-operation-dialog.scss',
})
export class BondOperationDialog {
  private readonly formFactory = inject(BondFormFactory);
  private readonly dialogRef = inject<MatDialogRef<BondOperationDialog, BondDetailsOperation>>(
    MatDialogRef,
  );

  private readonly data = inject<BondOperationDialogData | null>(MAT_DIALOG_DATA, {
    optional: true,
  });

  protected readonly operationTypes = OperationTypes;
  protected readonly platformTypes = PlatformTypes;

  protected readonly form: OperationFormGroup = this.formFactory.createOperationGroup(
    this.data?.operation,
  );

  protected onSave(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.dialogRef.close(this.form.getRawValue());
  }

  protected onCancel(): void {
    this.dialogRef.close();
  }
}
