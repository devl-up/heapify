import { useQuery } from "@tanstack/react-query";
import { createFileRoute } from "@tanstack/react-router";
import { meOptions } from "@/api/auth/queries";

export const Route = createFileRoute("/_authenticated/home/")({
  component: RouteComponent,
});

function RouteComponent() {
  const userQuery = useQuery(meOptions());
  return <div>Hello {userQuery.data?.name}!</div>;
}
