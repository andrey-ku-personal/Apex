import { Directive, Injector, inject } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Directive()
export abstract class FormControlAbstract<V> implements ControlValueAccessor {
  protected readonly injector = inject(Injector);

  protected value: V = null!;
  protected disabled = false;

  protected ngControl: NgControl = null!;

  get control(): FormControl {
    return this.ngControl?.control as FormControl;
  }

  get invalid(): boolean {
    return !!this.control?.invalid && !!this.control?.touched;
  }

  get errorMessage(): string | null {
    if (!this.control?.errors || !this.control?.touched) {
      return null;
    }

    const errors = this.control.errors;

    if (errors['required']) {
      return 'Поле обязательно для заполнения';
    }
    if (errors['email']) {
      return 'Введите корректный email';
    }
    if (errors['min']) {
      return `Минимальное значение: ${errors['min'].min}`;
    }
    if (errors['max']) {
      return `Максимальное значение: ${errors['max'].max}`;
    }
    if (errors['minlength']) {
      return `Минимум символов: ${errors['minlength'].requiredLength}`;
    }

    return 'Некорректное значение';
  }

  ngOnInit(): void {
    this.ngControl = this.injector.get(NgControl);
  }

  // === ControlValueAccessor ===

  public onChange: (value: V) => void = () => {};
  public onTouch: () => void = () => {};

  writeValue(value: V): void {
    this.value = value;
  }

  registerOnChange(fn: (value: V) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouch = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }
}