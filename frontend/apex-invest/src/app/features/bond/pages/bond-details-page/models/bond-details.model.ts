import { BondDetailsOperation } from "./bond-details-operations";

export interface BondDetails {
  id: number;
  platformId: number;
  ticker: string;
  issuer: string;
  currency: string;
  parPrice: number;
  couponRate: number;
  refinancingRate?: boolean;
  paymentFrequency: string;
  nextCouponDate: number;
  placementDate?: string;
  maturityDate: string;
  operations: BondDetailsOperation[];
}