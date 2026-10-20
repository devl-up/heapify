import { createFileRoute, Outlet } from "@tanstack/react-router";
import { getProjectOptions } from "@/api/projects/queries";

export const Route = createFileRoute("/_authenticated/projects/$id")({
  component: Outlet,
  loader: ({ context, params }) => context.queryClient.query(getProjectOptions(params.id)),
});
