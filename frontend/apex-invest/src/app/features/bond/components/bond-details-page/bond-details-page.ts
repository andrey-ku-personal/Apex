import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { BondMetadata } from './components/bond-metadata/bond-metadata';
import { BondSellBuyOperations } from './components/bond-sell-buy-operations/bond-sell-buy-operations';
import { BondStatus } from "./components/bond-status/bond-status";

@Component({
  selector: 'app-bond-details-page',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    BondMetadata,
    BondSellBuyOperations,
    BondStatus
],
  templateUrl: './bond-details-page.html',
  styleUrl: './bond-details-page.scss',
})
export class BondDetailsPage {
  private readonly fb = inject(FormBuilder);

  protected readonly form: FormGroup = this.fb.group({
    ticker: ['', [Validators.required]],
    issuer: ['', [Validators.required]],
    currency: ['BYN', [Validators.required]],
    parPrice: [null, [Validators.required, Validators.min(0)]],
    couponRate: [null, [Validators.required, Validators.min(0)]],
    paymentFrequency: ['', [Validators.required]],
    nextCouponDate: [null],
    maturityDate: [null, [Validators.required]],
    status: ['active', [Validators.required]],
    operations: this.fb.array([]),
  });

  protected onSubmit() {
    console.log(this.form.value);
  }
}