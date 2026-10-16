import { type VariantProps } from "class-variance-authority";
import { CircleAlert } from "lucide-react";
import { useMemo, type ComponentProps } from "react";
import { alertVariants } from "./alert.styles";
import { cn } from "@/lib/utils";

type AlertProps = ComponentProps<"div"> & VariantProps<typeof alertVariants>;

const Alert = ({ className, intent, children, ...props }: AlertProps) => {
  const icon = useMemo(() => {
    switch (intent) {
      case "error":
        return <CircleAlert />;
      default:
        return null;
    }
  }, [intent]);

  return (
    <div {...props} className={cn(alertVariants({ intent }), className)}>
      {icon}
      {children}
    </div>
  );
};

export { Alert };
