import { Component } from '@angular/core';
import { ControlContainer, FormGroupDirective, ReactiveFormsModule } from '@angular/forms';
import { MatCard, MatCardHeader, MatCardContent } from "@angular/material/card";
import { MatIcon } from "@angular/material/icon";
import { FormDate } from "../../../../../../shared/components/form/form-date/form-date";
import { FormSelect } from "../../../../../../shared/components/form/form-select/form-select";
import { BondStatuses } from '../../../../../../shared/options/bond-status.options';

@Component({
  selector: 'app-bond-status',
  viewProviders: [
    {
      provide: ControlContainer,
      useExisting: FormGroupDirective,
    },
  ],
  imports: [
    ReactiveFormsModule,
    MatCard,
    MatCardHeader,
    MatIcon,
    MatCardContent,
    FormDate,
    FormSelect
  ],
  templateUrl: './bond-status.html',
  styleUrl: './bond-status.scss',
})
export class BondStatus {
  protected readonly statuses = BondStatuses;
}
