import { OptionModel } from '../models/option.model';

export const Frequencies: OptionModel[] = [
  { value: 'monthly', label: 'Ежемесячно' },
  { value: 'quarterly', label: 'Ежеквартально' },
  { value: 'semiannual', label: 'Полугодовая' },
  { value: 'annual', label: 'Ежегодно' },
  { value: 'end', label: 'В конце срока' }
]

export const FrequencySteps: Record<string, number> = {
  monthly: 1,
  quarterly: 3,
  semiannual: 6,
  annual: 12,
};