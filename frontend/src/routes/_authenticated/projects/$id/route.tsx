import { createFileRoute, Outlet } from "@tanstack/react-router";
import { getProjectOptions } from "@/api/projects/queries";

export const Route = createFileRoute("/_authenticated/projects/$id")({
  component: Outlet,
  context: (context) => ({
    getProjectOptions: getProjectOptions(context.params.id),
  }),
  loader: async ({ context }) => {
    await context.queryClient.query(context.getProjectOptions);
  },
});
