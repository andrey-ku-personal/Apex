import { StatusOptionModel } from '../models/status-option.model';

export const BondStatuses: StatusOptionModel[] = [
  { value: 'active', label: 'Активна', color: '#16a34a' },
  { value: 'matured', label: 'Погашена', color: '#f59e0b' },
  { value: 'soldearly', label: 'Продана досрочно', color: '#475569' },
]