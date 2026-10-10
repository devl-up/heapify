import { createLink } from "@tanstack/react-router";
import { cva, type VariantProps } from "class-variance-authority";
import { cn } from "@/lib/utils";

import type { ComponentProps } from "react";

const navLinkVariants = cva(
  "flex items-center gap-1 transition-all disabled:pointer-events-none disabled:opacity-50",
  {
    variants: {
      variant: {
        default: "text-primary underline underline-offset-4",
        button:
          "rounded border border-primary bg-primary px-2 text-primary-foreground hover:bg-primary/80",
      },
      size: {
        default: "h-8 [&>svg]:size-3",
      },
    },
    defaultVariants: {
      variant: "default",
      size: "default",
    },
  },
);

type NavLinkProps = ComponentProps<"a"> & VariantProps<typeof navLinkVariants>;

const BaseNavLink = ({ className, variant, size, children, ...props }: NavLinkProps) => {
  return (
    <a {...props} className={cn(navLinkVariants({ variant, size }), className)}>
      {children}
    </a>
  );
};

const NavLink = createLink(BaseNavLink);

export default NavLink;
