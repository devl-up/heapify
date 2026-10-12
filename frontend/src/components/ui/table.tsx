import { cn } from "@/lib/utils";

import type { ComponentProps } from "react";

type TableProps = ComponentProps<"table">;

const Table = ({ className, ...props }: TableProps) => {
  return <table {...props} className={cn("table-fixed w-full", className)} />;
};

type TableHeadProps = ComponentProps<"thead">;

const TableHead = (props: TableHeadProps) => {
  return <thead {...props} />;
};

type TableBodyProps = ComponentProps<"tbody">;

const TableBody = (props: TableBodyProps) => {
  return <tbody {...props} />;
};

type TableRowProps = ComponentProps<"tr">;

const TableRow = (props: TableRowProps) => {
  return <tr {...props} />;
};

type TableHeaderProps = ComponentProps<"th">;

const TableHeader = ({ className, ...props }: TableHeaderProps) => {
  return <th {...props} className={cn("p-2 text-left border", className)} />;
};

type TableCellProps = ComponentProps<"td">;

const TableCell = ({ className, ...props }: TableCellProps) => {
  return <td {...props} className={cn("p-2 border", className)} />;
};

export { Table, TableHead, TableBody, TableRow, TableHeader, TableCell };
