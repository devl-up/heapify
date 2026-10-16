import { createLink } from "@tanstack/react-router";
import { buttonVariants } from "./button.styles";
import { cn } from "@/lib/utils";

import type { VariantProps } from "class-variance-authority";
import type { ComponentProps } from "react";

type ButtonLinkProps = ComponentProps<"a"> & VariantProps<typeof buttonVariants>;

const BaseButtonLink = ({ className, intent, size, children, ...props }: ButtonLinkProps) => {
  return (
    <a {...props} className={cn(buttonVariants({ intent, size }), className)}>
      {children}
    </a>
  );
};

const ButtonLink = createLink(BaseButtonLink);

export { ButtonLink };
