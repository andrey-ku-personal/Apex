import { TableCellType } from './table-cell-type';

export interface TableColumnConfig {
  field: string;
  label: string;
  type: TableCellType;
  dateFormat?: string;
  actions?: TableActionConfig[];
}

export interface TableActionConfig {
  key: string;
  icon: string;
  label?: string;
  class?: string;
}

export interface TableActionEvent<T = any> {
  key: string;
  row: T;
  index: number;
}
