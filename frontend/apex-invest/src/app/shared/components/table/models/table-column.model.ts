export enum TableColumnControl {
  Text = 'text',
  Number = 'number',
  Date = 'date',
}

export interface TableColumn {
  fieldCode: string;
  title: string;
  control?: TableColumnControl;
  dateFormat?: string;
}