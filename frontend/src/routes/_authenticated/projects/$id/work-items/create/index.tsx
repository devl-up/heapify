import { revalidateLogic } from "@tanstack/react-form";
import { useMutation } from "@tanstack/react-query";
import { createFileRoute } from "@tanstack/react-router";
import { ChevronLeft } from "lucide-react";
import { z } from "zod";
import { workItemsApi } from "@/api/work-items/api";
import { Alert } from "@/components/ui/alert";
import { NavLink } from "@/components/ui/nav-link";
import { useAppForm } from "@/hooks/use-app-form";

import type { CreateWorkItemRequest } from "@/api/work-items/types";

const schema = z.object({
  title: z
    .string()
    .trim()
    .min(1, { message: "Title is required" })
    .max(100, { message: "Title must be at most 100 characters long" }),
});

export const Route = createFileRoute("/_authenticated/projects/$id/work-items/create/")({
  component: RouteComponent,
});

function RouteComponent() {
  const navigate = Route.useNavigate();
  const params = Route.useParams();

  const createMutation = useMutation({
    mutationKey: ["create-work-item"],
    mutationFn: (request: CreateWorkItemRequest) => workItemsApi.create(request),
    onSuccess: async () => {
      await navigate({ to: "/projects/$id", params: { id: params.id } });
    },
  });

  const form = useAppForm({
    defaultValues: {
      title: "",
    },
    validationLogic: revalidateLogic({
      mode: "submit",
      modeAfterSubmission: "change",
    }),
    validators: {
      onDynamic: schema,
    },
    onSubmit: async ({ value }) => {
      await createMutation.mutateAsync({ projectId: params.id, title: value.title });
    },
  });

  return (
    <div className="flex flex-col gap-4">
      <NavLink to="/projects/$id" params={{ id: params.id }}>
        <ChevronLeft /> Back
      </NavLink>
      <h1 className="text-2xl">Create Work Item</h1>
      <form
        className="flex w-64 flex-col gap-4"
        onSubmit={async (e) => {
          e.preventDefault();
          await form.handleSubmit();
        }}
      >
        <form.AppField name="title">
          {(field) => <field.TextField label="Title" type="text" />}
        </form.AppField>
        {createMutation.isError && <Alert intent="error">Work item creation failed</Alert>}
        <div>
          <form.AppForm>
            <form.SubmitButton label="Save" />
          </form.AppForm>
        </div>
      </form>
    </div>
  );
}
