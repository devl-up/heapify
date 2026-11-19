import { useQuery } from "@tanstack/react-query";
import { createFileRoute } from "@tanstack/react-router";
import { Plus } from "lucide-react";
import z from "zod";
import { getProjectWorkItemsOptions } from "@/api/projects/queries";
import { ButtonLink } from "@/components/ui/button-link";
import { Pagination } from "@/components/ui/pagination";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { pageSchema } from "@/lib/page";

const searchSchema = z.object({ ...pageSchema.shape });

export const Route = createFileRoute("/_authenticated/projects/$id/")({
  component: RouteComponent,
  validateSearch: searchSchema,
  loaderDeps: ({ search }) => ({ index: search.index, size: search.size }),
  context: (context) => ({
    getWorkItemsOptions: getProjectWorkItemsOptions(context.params.id, {
      index: context.deps.index,
      size: context.deps.size,
    }),
  }),
  loader: async ({ context }) => {
    await context.queryClient.query(context.getWorkItemsOptions);
  },
});

function RouteComponent() {
  const { index, size } = Route.useSearch();
  const navigate = Route.useNavigate();
  const params = Route.useParams();
  const { getWorkItemsOptions } = Route.useRouteContext();

  const workItemsQuery = useQuery(getWorkItemsOptions);

  return (
    <div className="flex flex-col gap-4">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl">Work Items</h1>
        <ButtonLink to="/projects/$id/work-items/create" params={{ id: params.id }}>
          <Plus />
          Create Work Item
        </ButtonLink>
      </div>
      <Table>
        <TableHead>
          <TableRow>
            <TableHeader className="w-14 text-center">#</TableHeader>
            <TableHeader>Title</TableHeader>
          </TableRow>
        </TableHead>
        <TableBody>
          {workItemsQuery.data?.items?.map((workItem) => (
            <TableRow key={workItem.number}>
              <TableCell className="text-center">{workItem.number}</TableCell>
              <TableCell>{workItem.title}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
      <Pagination
        page={{ index, size }}
        onPaginationChange={async (page) => {
          await navigate({ search: page });
        }}
        totalCount={workItemsQuery.data?.totalCount ?? 0}
      />
    </div>
  );
}
