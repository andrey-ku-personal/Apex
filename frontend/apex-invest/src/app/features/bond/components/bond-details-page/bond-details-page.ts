import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-bond-details-page',
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatIconModule
  ],
  templateUrl: './bond-details-page.html',
  styleUrl: './bond-details-page.scss',
})
export class BondDetailsPage implements OnInit {
  private readonly fb = inject(FormBuilder);

  protected dataForm: FormGroup = null!;

  public ngOnInit(): void {
    this.dataForm = this.fb.group({
      ticker: ['', [Validators.required]],
      issuer: ['', [Validators.required]],
      currency: ['', [Validators.required]],
      faceValue: [null, [Validators.required, Validators.min(0)]],
      couponRate: [null, [Validators.required, Validators.min(0)]],
      paymentFrequency: ['', [Validators.required]],
      firstPayment: [null],
      maturityDate: [null, [Validators.required]]
    });
  }

  protected onSubmit() {
    console.log(this.dataForm.value);
  }
}
