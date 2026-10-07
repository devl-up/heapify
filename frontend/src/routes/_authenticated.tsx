import { useMutation, useQueryClient } from "@tanstack/react-query";
import { createFileRoute, Outlet, redirect } from "@tanstack/react-router";
import { authApi } from "@/api/auth/api";
import { meOptions } from "@/api/auth/queries";
import { Button } from "@/components/ui/button";

export const Route = createFileRoute("/_authenticated")({
  beforeLoad: async ({ context }) => {
    const user = await context.queryClient.query(meOptions()).catch(() => null);

    if (!user) {
      throw redirect({ to: "/login" });
    }
  },
  component: AuthenticatedComponent,
});

function AuthenticatedComponent() {
  const navigate = Route.useNavigate();
  const queryClient = useQueryClient();

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
        <span>Heapify</span>
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
