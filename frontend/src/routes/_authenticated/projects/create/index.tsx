import { revalidateLogic } from "@tanstack/react-form";
import { useMutation } from "@tanstack/react-query";
import { createFileRoute } from "@tanstack/react-router";
import { ChevronLeft } from "lucide-react";
import { z } from "zod";
import { projectApi } from "@/api/projects/api";
import { Alert } from "@/components/ui/alert";
import { NavLink } from "@/components/ui/nav-link";
import { useAppForm } from "@/hooks/use-app-form";

import type { CreateProjectRequest } from "@/api/projects/types";

const schema = z.object({
  name: z
    .string()
    .min(1, { message: "Name is required" })
    .max(50, { message: "Name must be at most 50 characters long" }),
});

export const Route = createFileRoute("/_authenticated/projects/create/")({
  component: RouteComponent,
});

function RouteComponent() {
  const navigate = Route.useNavigate();

  const createMutation = useMutation({
    mutationKey: ["create-project"],
    mutationFn: (request: CreateProjectRequest) => projectApi.create(request),
    onSuccess: async () => {
      await navigate({ to: "/projects" });
    },
  });

  const form = useAppForm({
    defaultValues: {
      name: "",
    },
    validationLogic: revalidateLogic({
      mode: "submit",
      modeAfterSubmission: "change",
    }),
    validators: {
      onDynamic: schema,
    },
    onSubmit: async ({ value }) => {
      await createMutation.mutateAsync(value);
    },
  });

  return (
    <div className="flex flex-col gap-4">
      <NavLink to="/projects">
        <ChevronLeft /> Back
      </NavLink>
      <h1 className="text-2xl">Create Project</h1>
      <form
        className="flex w-64 flex-col gap-4"
        onSubmit={async (e) => {
          e.preventDefault();
          await form.handleSubmit();
        }}
      >
        <form.AppField name="name">
          {(field) => <field.TextField label="Name" type="text" />}
        </form.AppField>
        {createMutation.isError && <Alert intent="error">Project creation failed</Alert>}
        <div>
          <form.AppForm>
            <form.SubmitButton label="Save" />
          </form.AppForm>
        </div>
      </form>
    </div>
  );
}
