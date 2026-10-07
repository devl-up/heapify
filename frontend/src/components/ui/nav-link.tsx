import { createLink } from "@tanstack/react-router";
import { cva, type VariantProps } from "class-variance-authority";
import { cn } from "@/lib/utils";

import type { ComponentProps } from "react";

const navLinkVariants = cva("transition-all disabled:pointer-events-none disabled:opacity-50", {
  variants: {
    variant: {
      default: "text-primary underline-offset-4 hover:underline",
    },
    size: {
      default: "h-8",
    },
  },
  defaultVariants: {
    variant: "default",
    size: "default",
  },
});

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
