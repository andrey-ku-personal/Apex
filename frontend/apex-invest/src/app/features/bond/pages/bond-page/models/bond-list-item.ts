import { BondDetailsOperation } from '../../bond-details-page/models/bond-details-operations';

export interface BondListItem {
  id: number;
  ticker: string;
  issuer: string;
  couponRate: number;
  currency: string;
  status: string;
  operations: BondDetailsOperation[];
}
