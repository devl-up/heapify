import { createFormHook } from "@tanstack/react-form";
import { SubmitButton, TextField } from "@/components/ui/form";
import { fieldContext, formContext } from "@/lib/form";

const { useAppForm } = createFormHook({
  fieldContext,
  formContext,
  fieldComponents: {
    TextField,
  },
  formComponents: {
    SubmitButton,
  },
});

export { useAppForm };
