import { createFileRoute } from "@tanstack/react-router";
import { Plus } from "lucide-react";
import NavLink from "@/components/ui/nav-link";

export const Route = createFileRoute("/_authenticated/projects/")({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <div className="flex flex-col gap-4">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl">Projects</h1>
        <NavLink to="/projects/create" variant="button">
          <Plus />
          Create Project
        </NavLink>
      </div>
    </div>
  );
}
