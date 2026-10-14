import { createFileRoute } from "@tanstack/react-router";
import { Plus } from "lucide-react";
import { z } from "zod";
import { getProjectsOptions } from "@/api/projects/queries";
import NavLink from "@/components/ui/nav-link";
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
  loader: ({ context, deps }) => context.queryClient.query(getProjectsOptions(deps)),
});

function RouteComponent() {
  const projectsQuery = Route.useLoaderData();

  return (
    <div className="flex flex-col gap-4">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl">Projects</h1>
        <NavLink to="/projects/create" variant="button">
          <Plus />
          Create Project
        </NavLink>
      </div>
      <Table>
        <TableHead>
          <TableRow>
            <TableHeader>ID</TableHeader>
            <TableHeader>Name</TableHeader>
          </TableRow>
        </TableHead>
        <TableBody>
          {projectsQuery.items.map((project) => (
            <TableRow key={project.id}>
              <TableCell>{project.id}</TableCell>
              <TableCell>{project.name}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  );
}
