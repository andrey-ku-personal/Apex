import { Component } from '@angular/core';
import { ControlContainer, FormGroupDirective, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { FormDate } from '../../../../../../shared/components/form/form-date/form-date';
import { FormNumber } from '../../../../../../shared/components/form/form-number/form-number';
import { FormSelect } from '../../../../../../shared/components/form/form-select/form-select';
import { FormText } from '../../../../../../shared/components/form/form-text/form-text';
import { BondStatuses } from '../../../../../../shared/options/bond-status.options';
import { Currencies } from '../../../../../../shared/options/currency.options';
import { Frequencies } from '../../../../../../shared/options/frequency.options';
@Component({
  selector: 'app-bond-metadata',
  viewProviders: [
    {
      provide: ControlContainer,
      useExisting: FormGroupDirective,
    },
  ],
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatIconModule,
    FormText,
    FormSelect,
    FormNumber,
    FormDate,
  ],
  templateUrl: './bond-metadata.html',
  styleUrl: './bond-metadata.scss',
})
export class BondMetadata {
  protected readonly currencies = Currencies;
  protected readonly frequencies = Frequencies;
  protected readonly statuses = BondStatuses;
}