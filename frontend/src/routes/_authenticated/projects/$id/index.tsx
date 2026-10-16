import { createFileRoute } from "@tanstack/react-router";
import { getProjectOptions } from "@/api/projects/queries";

export const Route = createFileRoute("/_authenticated/projects/$id/")({
  component: RouteComponent,
  loader: ({ context, params }) => context.queryClient.query(getProjectOptions(params.id)),
});

function RouteComponent() {
  const projectQuery = Route.useLoaderData();

  return <div>Hello {projectQuery?.name}!</div>;
}
