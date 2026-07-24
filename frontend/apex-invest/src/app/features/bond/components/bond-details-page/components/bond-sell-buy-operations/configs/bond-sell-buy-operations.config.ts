import { TableCellType } from '../../../../../../../shared/models/table-cell-type';
import { TableColumnConfig } from '../../../../../../../shared/models/table.model';

export const BOND_OPERATIONS_TABLE_COLUMNS: TableColumnConfig[] = [
  { field: 'type', label: 'Тип', type: TableCellType.Text },
  { field: 'date', label: 'Дата', type: TableCellType.Date },
  { field: 'price', label: 'Цена, Br', type: TableCellType.Number },
  { field: 'count', label: 'Кол-во', type: TableCellType.Number },
  { field: 'sum', label: 'Сумма', type: TableCellType.Number },
  {
    field: 'actions',
    label: '',
    type: TableCellType.Actions,
    actions: [
      { key: 'delete', icon: 'delete', label: 'Удалить', class: 'btn-icon-danger' },
    ],
  },
];