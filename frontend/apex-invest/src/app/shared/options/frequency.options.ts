import { OptionModel } from '../models/option.model';

export const Frequencies: OptionModel[] = [
  { value: 'monthly', label: 'Ежемесячно' },
  { value: 'quarterly', label: 'Ежеквартально' },
  { value: 'semiannual', label: 'Полугодовая' },
  { value: 'annual', label: 'Ежегодно' },
  { value: 'end', label: 'В конце срока' }
]