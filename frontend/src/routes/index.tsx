import { useQuery } from "@tanstack/react-query";
import { createFileRoute, Navigate } from "@tanstack/react-router";
import { meOptions } from "@/api/auth/queries";

export const Route = createFileRoute("/")({
  component: IndexComponent,
});

function IndexComponent() {
  const userQuery = useQuery(meOptions());

  if (userQuery.isPending) {
    return <div>Authenticating...</div>;
  }

  if (userQuery.isError || !userQuery.data) {
    return <Navigate to="/login" replace />;
  }

  return <Navigate to="/home" replace />;
}
