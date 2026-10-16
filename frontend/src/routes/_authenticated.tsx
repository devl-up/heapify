import { useMutation, useQueryClient } from "@tanstack/react-query";
import { createFileRoute, Outlet, redirect, useMatch } from "@tanstack/react-router";
import { authApi } from "@/api/auth/api";
import { meOptions } from "@/api/auth/queries";
import { Button } from "@/components/ui/button";
import { ButtonLink } from "@/components/ui/button-link";

export const Route = createFileRoute("/_authenticated")({
  beforeLoad: async ({ context }) => {
    const user = await context.queryClient.query(meOptions()).catch(() => null);

    if (!user) {
      throw redirect({ to: "/login" });
    }
  },
  loader: ({ context }) => {
    return context.queryClient.query(meOptions());
  },
  component: AuthenticatedComponent,
});

function AuthenticatedComponent() {
  const navigate = Route.useNavigate();
  const queryClient = useQueryClient();

  const userQuery = Route.useLoaderData();

  const projectMatch = useMatch({ from: "/_authenticated/projects/$id/", shouldThrow: false });
  const project = projectMatch?.loaderData;

  const logoutMutation = useMutation({
    mutationKey: ["logout"],
    mutationFn: authApi.logout,
    onSuccess: async () => {
      queryClient.setQueryData(meOptions().queryKey, null);
      await navigate({ to: "/login" });
    },
  });

  return (
    <div className="flex min-h-screen flex-col gap-4 divide-y">
      <div className="flex items-center justify-between px-4 py-2">
        <div className="flex items-center gap-1">
          <ButtonLink to="/" intent="ghost">
            {userQuery?.name}
          </ButtonLink>
          {project && (
            <>
              <span>/</span>
              <ButtonLink to="/projects/$id" params={{ id: project.id }} intent="ghost">
                {project.name}
              </ButtonLink>
            </>
          )}
        </div>

        <Button
          disabled={logoutMutation.isPending}
          onClick={async () => await logoutMutation.mutateAsync()}
        >
          {logoutMutation.isPending ? "Logging out..." : "Logout"}
        </Button>
      </div>
      <main className="flex-1 p-4">
        <Outlet />
      </main>
    </div>
  );
}
