import { useQuery } from "@tanstack/react-query";
import { createFileRoute } from "@tanstack/react-router";
import { Eye, Plus } from "lucide-react";
import { z } from "zod";
import { getProjectsOptions } from "@/api/projects/queries";
import { ButtonLink } from "@/components/ui/button-link";
import { NavLink } from "@/components/ui/nav-link";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  DEFAULT_PAGE_INDEX,
  DEFAULT_PAGE_SIZE,
  INDEX_MIN_VALUE,
  PAGE_MAX_VALUE,
  PAGE_MIN_VALUE,
} from "@/lib/page";

const searchSchema = z.object({
  index: z.number().gte(INDEX_MIN_VALUE).default(DEFAULT_PAGE_INDEX),
  size: z.number().gte(PAGE_MIN_VALUE).lte(PAGE_MAX_VALUE).default(DEFAULT_PAGE_SIZE),
});

export const Route = createFileRoute("/_authenticated/projects/")({
  component: RouteComponent,
  validateSearch: searchSchema,
  loaderDeps: ({ search }) => ({ index: search.index, size: search.size }),
  context: ({ deps }) => ({
    getProjectsOptions: getProjectsOptions(deps),
  }),
  loader: async ({ context }) => {
    await context.queryClient.query(context.getProjectsOptions);
  },
});

function RouteComponent() {
  const { getProjectsOptions } = Route.useRouteContext();
  const projectsQuery = useQuery(getProjectsOptions);

  return (
    <div className="flex flex-col gap-4">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl">Projects</h1>
        <ButtonLink to="/projects/create">
          <Plus />
          Create Project
        </ButtonLink>
      </div>
      <Table>
        <TableHead>
          <TableRow>
            <TableHeader>Name</TableHeader>
            <TableHeader className="w-14" />
          </TableRow>
        </TableHead>
        <TableBody>
          {projectsQuery.data?.items?.map((project) => (
            <TableRow key={project.id}>
              <TableCell>{project.name}</TableCell>
              <TableCell>
                <NavLink to="/projects/$id" params={{ id: project.id }} intent="icon">
                  <Eye />
                </NavLink>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  );
}
