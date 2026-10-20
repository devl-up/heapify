import { createFileRoute } from "@tanstack/react-router";
import { Plus } from "lucide-react";
import { ButtonLink } from "@/components/ui/button-link";

export const Route = createFileRoute("/_authenticated/projects/$id/")({
  component: RouteComponent,
});

function RouteComponent() {
  const params = Route.useParams();

  return (
    <div className="flex items-center justify-end">
      <ButtonLink to="/projects/$id/work-items/create" params={{ id: params.id }}>
        <Plus />
        Create Work Item
      </ButtonLink>
    </div>
  );
}
