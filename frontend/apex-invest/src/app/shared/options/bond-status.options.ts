import { OptionModel } from '../models/option.model';

export const BondStatuses: OptionModel[] = [
  { value: 'active', label: 'Активна' },
  { value: 'matured', label: 'Погашена' },
  { value: 'sold', label: 'Продана досрочно' },
]