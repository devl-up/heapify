import { useQuery } from "@tanstack/react-query";
import { createFileRoute } from "@tanstack/react-router";
import { getWorkItemOptions } from "@/api/work-items/queries";

export const Route = createFileRoute(
  "/_authenticated/projects/$projectId/work-items/$workItemNumber/",
)({
  component: RouteComponent,
  context: (context) => ({
    getWorkItemOptions: getWorkItemOptions(
      context.params.projectId,
      Number(context.params.workItemNumber),
    ),
  }),
  loader: async ({ context }) => {
    await context.queryClient.query(context.getWorkItemOptions);
  },
});

function RouteComponent() {
  const { getWorkItemOptions } = Route.useRouteContext();
  const workItemQuery = useQuery(getWorkItemOptions);

  return (
    <div className="flex flex-col gap-4">
      <h1 className="text-2xl">
        #{workItemQuery.data?.number} - {workItemQuery.data?.title}
      </h1>
    </div>
  );
}
