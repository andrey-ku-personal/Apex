import { Component, computed, effect, inject, resource, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import { firstValueFrom, map } from 'rxjs';
import { BondMetadata } from './components/bond-metadata/bond-metadata';
import { BondFormGroup } from './form-groups/bond-form-group';
import { BondDetails } from './models/bond-details.model';
import { BondDetailsApiService } from './services/bond-details-api.service';
import { BondSellBuyOperations } from './components/bond-sell-buy-operations/bond-sell-buy-operations';
import { BondSummary } from './components/bond-summary/bond-summary';
import { BondPaymentSchedule } from './components/bond-payment-schedule/bond-payment-schedule';
import { BondFormFactory } from './services/bond-form.factory';

@Component({
  selector: 'app-bond-details-page',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    BondMetadata,
    BondSellBuyOperations,
    BondSummary,
    BondPaymentSchedule
  ],
  providers: [BondDetailsApiService],
  templateUrl: './bond-details-page.html',
  styleUrl: './bond-details-page.scss',
})
export class BondDetailsPage {
  private readonly formFactory = inject(BondFormFactory);
  private readonly apiService = inject(BondDetailsApiService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  private readonly routeId = toSignal(
    this.route.paramMap.pipe(map((p) => Number(p.get('id') ?? 0))),
    { initialValue: 0 },
  );
  private readonly isLoading = signal(false);

  readonly bondResource = resource<BondDetails | undefined, number>({
    params: () => this.routeId(),
    loader: ({ params: id }) =>
      id > 0 ? firstValueFrom(this.apiService.get(id)) : Promise.resolve(undefined),
  });

  readonly loading = computed(() => this.bondResource.isLoading() || this.isLoading());

  protected form: BondFormGroup = this.formFactory.createBondForm();

  constructor() {
    effect(() => {
      const data = this.bondResource.value();
      if (data) {
        const { operations, ...rest } = data;
        this.form.patchValue(rest);

        const array = this.form.controls.operations;
        array.clear();
        for (const operation of operations) {
          array.push(this.formFactory.createOperationGroup(operation));
        }
      }
    });
  }

  protected async onSubmit(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);
    try {
      const raw = this.form.getRawValue();
      const request = raw.id > 0 ? this.apiService.update(raw) : this.apiService.create(raw);
      const result = await firstValueFrom(request);
      this.router.navigate(['/bond', result.id]);
    } catch (err) {
      console.error('Save failed:', err);
    } finally {
      this.isLoading.set(false);
    }
  }
}