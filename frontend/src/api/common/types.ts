export interface PaginatedResponse<T> {
  readonly items: T[];
  readonly totalCount: number;
}
