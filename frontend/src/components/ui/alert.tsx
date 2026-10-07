import { cva, type VariantProps } from "class-variance-authority";
import { CircleAlert } from "lucide-react";
import { useMemo, type ComponentProps } from "react";
import { cn } from "@/lib/utils";

const alertVariants = cva(
  "flex items-center gap-2 rounded-sm border px-2 py-1 text-sm [&_svg]:size-4",
  {
    variants: {
      variant: {
        error: "border-red-500 text-red-500",
      },
    },
  },
);

type AlertProps = ComponentProps<"div"> & VariantProps<typeof alertVariants>;

const Alert = ({ className, variant, children, ...props }: AlertProps) => {
  const icon = useMemo(() => {
    switch (variant) {
      case "error":
        return <CircleAlert />;
      default:
        return null;
    }
  }, [variant]);

  return (
    <div {...props} className={cn(alertVariants({ variant }), className)}>
      {icon}
      {children}
    </div>
  );
};

export default Alert;
