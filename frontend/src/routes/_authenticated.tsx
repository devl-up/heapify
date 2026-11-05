import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { createFileRoute, Outlet, redirect, useMatch } from "@tanstack/react-router";
import { ProjectBreadcrumb } from "./-components/project-breadcrumb";
import { authApi } from "@/api/auth/api";
import { meOptions } from "@/api/auth/queries";
import { Button } from "@/components/ui/button";
import { ButtonLink } from "@/components/ui/button-link";

export const Route = createFileRoute("/_authenticated")({
  context: () => ({
    meOptions: meOptions(),
  }),
  beforeLoad: async ({ context }) => {
    const user = await context.queryClient.query(context.meOptions).catch(() => null);

    if (!user) {
      throw redirect({ to: "/login" });
    }
  },
  component: AuthenticatedComponent,
});

function AuthenticatedComponent() {
  const navigate = Route.useNavigate();
  const queryClient = useQueryClient();

  const { meOptions } = Route.useRouteContext();
  const userQuery = useQuery(meOptions);

  const projectMatch = useMatch({ from: "/_authenticated/projects/$id", shouldThrow: false });

  const logoutMutation = useMutation({
    mutationKey: ["logout"],
    mutationFn: authApi.logout,
    onSuccess: async () => {
      queryClient.setQueryData(meOptions.queryKey, null);
      await navigate({ to: "/login" });
    },
  });

  return (
    <div className="flex min-h-screen flex-col gap-4 divide-y">
      <div className="flex items-center justify-between px-4 py-2">
        <div className="flex items-center gap-1">
          <ButtonLink to="/" intent="ghost">
            {userQuery.data?.name}
          </ButtonLink>
          {projectMatch && <ProjectBreadcrumb />}
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
