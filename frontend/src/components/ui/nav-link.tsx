import { createLink } from "@tanstack/react-router";
import { type VariantProps } from "class-variance-authority";
import { navLinkVariants } from "./nav-link.styles";
import { cn } from "@/lib/utils";

import type { ComponentProps } from "react";

type NavLinkProps = ComponentProps<"a"> & VariantProps<typeof navLinkVariants>;

const BaseNavLink = ({ className, intent, children, ...props }: NavLinkProps) => {
  return (
    <a {...props} className={cn(navLinkVariants({ intent }), className)}>
      {children}
    </a>
  );
};

const NavLink = createLink(BaseNavLink);

export { NavLink };
