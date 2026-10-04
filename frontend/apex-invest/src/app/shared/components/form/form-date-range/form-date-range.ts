import { Component, input, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { format, isValid, parse, startOfDay } from 'date-fns';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-form-date-range',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatDatepickerModule, MatIconModule],
  templateUrl: './form-date-range.html',
  styleUrl: './form-date-range.scss',
})
export class FormDateRange implements OnInit, OnDestroy {
  public readonly label = input<string>('');
  public readonly startPlaceholder = input<string>('Начало');
  public readonly endPlaceholder = input<string>('Конец');
  public readonly isRequired = input<boolean>(false);
  public readonly note = input<string>('');

  public readonly startControl = input.required<FormControl<string>>();
  public readonly endControl = input.required<FormControl<string>>();

  protected readonly internal = new FormGroup({
    start: new FormControl<Date | null>(null),
    end: new FormControl<Date | null>(null),
  });

  private readonly subscription = new Subscription();

  public ngOnInit(): void {
    const start = this.startControl();
    const end = this.endControl();

    this.internal.controls.start.setValue(this.parseDate(start.value), { emitEvent: false });
    this.internal.controls.end.setValue(this.parseDate(end.value), { emitEvent: false });

    this.subscription.add(
      start.valueChanges.subscribe((value) => this.syncInternal('start', value)),
    );
    this.subscription.add(
      end.valueChanges.subscribe((value) => this.syncInternal('end', value)),
    );
  }

  public ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  protected get errorMessage(): string | null {
    const start = this.startControl();
    const end = this.endControl();

    if (start.touched && start.errors?.['required']) {
      return 'Укажите дату начала периода';
    }
    if (end.touched && end.errors?.['required']) {
      return 'Укажите дату окончания периода';
    }

    return null;
  }

  protected onStartDateChange(): void {
    this.emitChange('start');
  }

  protected onEndDateChange(): void {
    this.emitChange('end');
  }

  private emitChange(key: 'start' | 'end'): void {
    const host = key === 'start' ? this.startControl() : this.endControl();

    host.markAsTouched();

    const formatted = this.formatDate(this.internal.value[key]);
    if (formatted !== host.value) {
      host.setValue(formatted);
    }
  }

  private syncInternal(key: 'start' | 'end', value: string | null): void {
    const control = this.internal.controls[key];
    const date = this.parseDate(value);

    if ((date?.getTime() ?? null) !== (control.value?.getTime() ?? null)) {
      control.setValue(date, { emitEvent: false });
    }
  }

  private formatDate(date: Date | null | undefined): string {
    return date && isValid(date) ? format(date, 'yyyy-MM-dd') : '';
  }

  private parseDate(value: string | null | undefined): Date | null {
    if (!value) {
      return null;
    }

    const date = parse(value.slice(0, 10), 'yyyy-MM-dd', new Date());
    return isValid(date) ? startOfDay(date) : null;
  }
}
