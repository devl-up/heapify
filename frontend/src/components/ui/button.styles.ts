import { cva } from "class-variance-authority";

const buttonVariants = cva(
  "flex cursor-pointer items-center justify-center gap-1 rounded-md transition-all disabled:pointer-events-none disabled:opacity-50",
  {
    variants: {
      intent: {
        primary: "border border-primary bg-primary text-primary-foreground hover:bg-primary/80",
        ghost: "text-primary hover:bg-primary/5",
        icon: "rounded-full p-0! hover:bg-primary/10",
      },
      size: {
        small: "h-8 px-2 [&>svg]:size-3",
        medium: "h-10 px-4 [&>svg]:size-4",
      },
    },
    compoundVariants: [
      { intent: "icon", size: "small", className: "size-8 p-0" },
      { intent: "icon", size: "medium", className: "size-10 p-0" },
    ],
    defaultVariants: {
      intent: "primary",
      size: "small",
    },
  },
);

export { buttonVariants };
