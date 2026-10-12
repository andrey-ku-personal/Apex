import { OptionModel } from '../models/option.model';

export const Frequencies: OptionModel[] = [
  { value: 'Monthly', label: 'Ежемесячно' },
  { value: 'Quarterly', label: 'Ежеквартально' },
  { value: 'Semiannual', label: 'Полугодовая' },
  { value: 'Annual', label: 'Ежегодно' },
  { value: 'End', label: 'В конце срока' }
]

export const FrequencySteps: Record<string, number> = {
  monthly: 1,
  quarterly: 3,
  semiannual: 6,
  annual: 12,
};