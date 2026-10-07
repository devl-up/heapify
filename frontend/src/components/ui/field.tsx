import { Field as BaseField } from "@base-ui/react/field";
import { cn } from "@/lib/utils";

import type { ComponentProps } from "react";

type FieldProps = ComponentProps<typeof BaseField.Root>;

const Field = ({ className, children, ...props }: FieldProps) => {
  return (
    <BaseField.Root {...props} className={cn("", className)}>
      {children}
    </BaseField.Root>
  );
};

type FieldLabelProps = ComponentProps<typeof BaseField.Label>;

const FieldLabel = ({ className, ...props }: FieldLabelProps) => {
  return <BaseField.Label {...props} className={cn("", className)} />;
};

type FieldControlProps = ComponentProps<typeof BaseField.Control>;

const FieldControl = ({ className, ...props }: FieldControlProps) => {
  return (
    <BaseField.Control
      {...props}
      className={cn(
        "outline-none border rounded-sm px-2 py-1 aria-invalid:border-red-500 aria-invalid:ring-1 aria-invalid:ring-red-500",
        className,
      )}
    />
  );
};

type FieldErrorProps = ComponentProps<typeof BaseField.Error>;

const FieldError = ({ className, ...props }: FieldErrorProps) => {
  return <BaseField.Error {...props} className={cn("text-red-500 text-sm", className)} />;
};

export { Field, FieldControl, FieldError, FieldLabel };
