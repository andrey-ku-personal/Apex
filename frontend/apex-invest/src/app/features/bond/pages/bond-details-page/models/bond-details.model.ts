import { BondDetailsOperation } from "./bond-details-operations";

export interface BondDetails {
  id: number;
  platformId: number;
  ticker: string;
  issuer: string;
  currency: string;
  parPrice: number;
  couponRate: number;
  paymentFrequency: string;
  nextCouponDate: number;
  maturityDate: string;
  status: string;
  operations: BondDetailsOperation[];
}