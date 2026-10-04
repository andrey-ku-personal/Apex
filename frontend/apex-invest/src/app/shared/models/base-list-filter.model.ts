export interface BaseListFilter {
  pageNumber: number;
  pageSize: number;
  sortBy?: string | null;
  isAscending: boolean;
}
