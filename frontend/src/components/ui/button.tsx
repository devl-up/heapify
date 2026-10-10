import { Button as BaseButton } from "@base-ui/react/button";
import { cva, type VariantProps } from "class-variance-authority";
import { cn } from "@/lib/utils";

import type { ComponentProps } from "react";

const buttonVariants = cva(
  "flex cursor-pointer items-center gap-1 rounded transition-all disabled:pointer-events-none disabled:opacity-50",
  {
    variants: {
      variant: {
        default: "border border-primary bg-primary text-primary-foreground hover:bg-primary/80",
      },
      size: {
        default: "h-8 px-2 [&>svg]:size-3",
      },
    },
    defaultVariants: {
      variant: "default",
      size: "default",
    },
  },
);

type ButtonProps = ComponentProps<typeof BaseButton> & VariantProps<typeof buttonVariants>;

const Button = ({ className, variant, size, children, ...props }: ButtonProps) => {
  return (
    <BaseButton {...props} className={cn(buttonVariants({ variant, size }), className)}>
      {children}
    </BaseButton>
  );
};

export { Button };
