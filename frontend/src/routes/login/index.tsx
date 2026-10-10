import { revalidateLogic } from "@tanstack/react-form";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { createFileRoute } from "@tanstack/react-router";
import { CircleAlert } from "lucide-react";
import { z } from "zod";
import { authApi } from "@/api/auth/api";
import { meOptions } from "@/api/auth/queries";
import Alert from "@/components/ui/alert";
import NavLink from "@/components/ui/nav-link";
import { useAppForm } from "@/hooks/use-app-form";

const schema = z.object({
  email: z.email({ message: "Invalid email address" }),
  password: z.string().min(8, { message: "Password must be at least 8 characters long" }),
});

export const Route = createFileRoute("/login/")({
  component: RouteComponent,
});

function RouteComponent() {
  const navigate = Route.useNavigate();
  const queryClient = useQueryClient();

  const loginMutation = useMutation({
    mutationKey: ["login"],
    mutationFn: authApi.login,
    onSuccess: async () => {
      await queryClient.resetQueries({ queryKey: meOptions().queryKey });
      await navigate({ to: "/" });
    },
  });

  const form = useAppForm({
    defaultValues: {
      email: "",
      password: "",
    },
    validationLogic: revalidateLogic({
      mode: "submit",
      modeAfterSubmission: "change",
    }),
    validators: {
      onDynamic: schema,
    },
    onSubmit: async ({ value }) => {
      await loginMutation.mutateAsync(value);
    },
  });

  return (
    <div className="flex min-h-screen items-center justify-center">
      <form
        className="flex w-64 flex-col gap-4"
        onSubmit={(e) => {
          e.preventDefault();
          form.handleSubmit();
        }}
      >
        <h1 className="self-center text-2xl">Login</h1>
        <form.AppField
          name="email"
          children={(field) => <field.TextField label="Email" type="email" />}
        />
        <form.AppField
          name="password"
          children={(field) => <field.TextField label="Password" type="password" />}
        />
        {loginMutation.isError && <Alert variant="error">Login failed</Alert>}
        <div className="flex flex-col gap-1">
          <form.AppForm>
            <form.SubmitButton label="Login" />
          </form.AppForm>
          <NavLink className="self-center" to="/register">
            Register
          </NavLink>
        </div>
      </form>
    </div>
  );
}
