import { cva } from "class-variance-authority";

const alertVariants = cva(
  "flex items-center gap-2 rounded-md border px-2 py-1 text-sm [&_svg]:size-4",
  {
    variants: {
      intent: {
        error: "border-red-500 text-red-500",
      },
    },
  },
);

export { alertVariants };
