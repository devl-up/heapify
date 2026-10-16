import { Button as BaseButton } from "@base-ui/react/button";
import { type VariantProps } from "class-variance-authority";
import { buttonVariants } from "./button.styles";
import { cn } from "@/lib/utils";

import type { ComponentProps } from "react";

type ButtonProps = ComponentProps<typeof BaseButton> & VariantProps<typeof buttonVariants>;

const Button = ({ className, intent, size, children, ...props }: ButtonProps) => {
  return (
    <BaseButton {...props} className={cn(buttonVariants({ intent, size }), className)}>
      {children}
    </BaseButton>
  );
};

export { Button };
