import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'objectKeys',
  standalone: true
})

export class ObjectKeysPipe implements PipeTransform {
  transform(value: Record<string, any> | null | undefined): string[] {
    if (!value) return [];
    return Object.keys(value);
  }
}