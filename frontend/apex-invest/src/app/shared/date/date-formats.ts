import { MatDateFormats } from '@angular/material/core';
import { MAT_DATE_FNS_FORMATS } from '@angular/material-date-fns-adapter';

export const RU_DATE_FORMATS: MatDateFormats = {
  parse: {
    ...MAT_DATE_FNS_FORMATS.parse,
    dateInput: ['dd.MM.yyyy', 'd.M.yyyy'],
  },
  display: {
    ...MAT_DATE_FNS_FORMATS.display,
    dateInput: 'dd.MM.yyyy',
  },
};