import { revalidateLogic } from "@tanstack/react-form";
import { useMutation } from "@tanstack/react-query";
import { createFileRoute } from "@tanstack/react-router";
import { z } from "zod";
import { authApi } from "@/api/auth/api";
import Alert from "@/components/ui/alert";
import NavLink from "@/components/ui/nav-link";
import { useAppForm } from "@/hooks/use-app-form";

const schema = z.object({
  username: z.string().min(1, { message: "Invalid username" }),
  email: z.email({ message: "Invalid email address" }),
  password: z.string().min(8, { message: "Password must be at least 8 characters long" }),
});

export const Route = createFileRoute("/register/")({
  component: RouteComponent,
});

function RouteComponent() {
  const navigate = Route.useNavigate();
  const registerMutation = useMutation({
    mutationKey: ["register"],
    mutationFn: authApi.register,
    onSuccess: async () => {
      await navigate({ to: "/login" });
    },
  });

  const form = useAppForm({
    defaultValues: {
      username: "",
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
      await registerMutation.mutateAsync(value);
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
        <h1 className="self-center text-2xl">Register</h1>
        <form.AppField
          name="username"
          children={(field) => <field.TextField label="Username" type="text" />}
        />
        <form.AppField
          name="email"
          children={(field) => <field.TextField label="Email" type="email" />}
        />
        <form.AppField
          name="password"
          children={(field) => <field.TextField label="Password" type="password" />}
        />
        {registerMutation.isError && <Alert variant="error">Registration failed</Alert>}
        <div className="flex flex-col gap-1">
          <form.AppForm>
            <form.SubmitButton label="Register" />
          </form.AppForm>
          <NavLink className="self-center" to="/login">
            Login
          </NavLink>
        </div>
      </form>
    </div>
  );
}
