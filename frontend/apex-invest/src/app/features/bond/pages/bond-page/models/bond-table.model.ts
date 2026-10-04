export interface BondTableModel {
  id: number;
  ticker: string;
  issuer: string;
  couponRate: number;
  currencyLabel: string;
  price: number;
  quantity: number;
  sum: number;
  status: string;
}
