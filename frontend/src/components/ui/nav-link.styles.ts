import { cva } from "class-variance-authority";

const navLinkVariants = cva(
  "flex items-center gap-1 transition-all disabled:pointer-events-none disabled:opacity-50",
  {
    variants: {
      intent: {
        inline: "text-primary underline underline-offset-4",
        ghost: "text-primary hover:text-primary/80",
        icon: "flex size-10 items-center justify-center rounded-full hover:bg-primary/10 [&>svg]:size-4",
      },
    },
    defaultVariants: {
      intent: "inline",
    },
  },
);

export { navLinkVariants };
