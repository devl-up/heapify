import { Button } from "./button";
import { Field, FieldControl, FieldError, FieldLabel } from "./field";
import { useFieldContext, useFormContext } from "@/lib/form";

type TextFieldProps = {
  readonly label: string;
  readonly type?: "text" | "email" | "password";
};

const TextField = ({ label, type }: TextFieldProps) => {
  const field = useFieldContext<string>();

  return (
    <Field
      className="flex flex-col gap-1"
      name={field.name}
      invalid={!field.state.meta.isValid}
      touched={field.state.meta.isTouched}
      dirty={field.state.meta.isDirty}
    >
      <FieldLabel>{label}</FieldLabel>
      <FieldControl type={type} value={field.state.value} onValueChange={field.handleChange} />
      {field.state.meta.errors.length > 0 && (
        <FieldError match={field.state.meta.errors.length > 0}>
          {field.state.meta.errors[0]?.message}
        </FieldError>
      )}
    </Field>
  );
};

type SubmitButtonProps = {
  readonly label: string;
};

const SubmitButton = ({ label }: SubmitButtonProps) => {
  const form = useFormContext();

  return (
    <form.Subscribe selector={(state) => [state.canSubmit, state.isSubmitting]}>
      {([canSubmit, isSubmitting]) => (
        <Button type="submit" disabled={!canSubmit || isSubmitting}>
          {label}
        </Button>
      )}
    </form.Subscribe>
  );
};

export { SubmitButton, TextField };
