import { createFileRoute, Outlet } from "@tanstack/react-router";
import z from "zod";
import { getProjectOptions, getProjectWorkItemsOptions } from "@/api/projects/queries";
import {
  DEFAULT_PAGE_INDEX,
  INDEX_MIN_VALUE,
  PAGE_MIN_VALUE,
  PAGE_MAX_VALUE,
  DEFAULT_PAGE_SIZE,
} from "@/lib/page";

const searchSchema = z.object({
  index: z.number().gte(INDEX_MIN_VALUE).default(DEFAULT_PAGE_INDEX),
  size: z.number().gte(PAGE_MIN_VALUE).lte(PAGE_MAX_VALUE).default(DEFAULT_PAGE_SIZE),
});

export const Route = createFileRoute("/_authenticated/projects/$id")({
  component: Outlet,
  validateSearch: searchSchema,
  loaderDeps: ({ search }) => ({ index: search.index, size: search.size }),
  context: (context) => ({
    getProjectOptions: getProjectOptions(context.params.id),
    getWorkItemsOptions: getProjectWorkItemsOptions(context.params.id, {
      index: context.deps.index,
      size: context.deps.size,
    }),
  }),
  loader: async ({ context }) => {
    await context.queryClient.query(context.getProjectOptions);
    await context.queryClient.query(context.getWorkItemsOptions);
  },
});
