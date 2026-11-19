import { z } from "zod";

export const DEFAULT_PAGE_INDEX = 0;
export const DEFAULT_PAGE_SIZE = 50;
export const INDEX_MIN_VALUE = 0;
export const PAGE_MIN_VALUE = 1;
export const PAGE_MAX_VALUE = 50;

export const pageSchema = z.object({
  index: z.int().gte(INDEX_MIN_VALUE).default(DEFAULT_PAGE_INDEX),
  size: z.int().gte(PAGE_MIN_VALUE).lte(PAGE_MAX_VALUE).default(DEFAULT_PAGE_SIZE),
});
