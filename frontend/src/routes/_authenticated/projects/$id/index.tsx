import { createFileRoute } from "@tanstack/react-router";
import { ChevronLeft } from "lucide-react";
import { getProjectOptions } from "@/api/projects/queries";
import NavLink from "@/components/ui/nav-link";

export const Route = createFileRoute("/_authenticated/projects/$id/")({
  component: RouteComponent,
  loader: ({ context, params }) => context.queryClient.query(getProjectOptions(params.id)),
});

function RouteComponent() {
  const projectQuery = Route.useLoaderData();

  return (
    <div className="flex flex-col gap-4">
      <NavLink to="/projects">
        <ChevronLeft /> Back
      </NavLink>
      <h1 className="text-2xl">{projectQuery.name}</h1>
    </div>
  );
}
