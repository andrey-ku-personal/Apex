export interface BondDetailsOperation {
  type: string;
  platformId?: number;
  date: string;
  price: number;
  count: number;
  comment?: string;
}