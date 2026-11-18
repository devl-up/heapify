import { ChevronLeft, ChevronRight } from "lucide-react";
import { useCallback, useMemo } from "react";
import { Button } from "./button";

interface PaginationProps {
  readonly page: {
    index: number;
    size: number;
  };
  readonly totalCount: number;
  readonly onPaginationChange: (page: { index: number; size: number }) => Promise<void>;
}

const Pagination = ({ totalCount, page, onPaginationChange }: PaginationProps) => {
  const hasPrevious = useMemo(() => page.index > 0, [page.index]);

  const hasNext = useMemo(
    () => page.size > 0 && page.index < Math.ceil(totalCount / page.size) - 1,
    [page.index, totalCount, page.size],
  );

  const previous = useCallback(async () => {
    if (!hasPrevious) return;
    await onPaginationChange({ index: page.index - 1, size: page.size });
  }, [hasPrevious, onPaginationChange, page.index, page.size]);

  const next = useCallback(async () => {
    if (!hasNext) return;
    await onPaginationChange({ index: page.index + 1, size: page.size });
  }, [hasNext, onPaginationChange, page.index, page.size]);

  return (
    <div className="flex items-center justify-between border p-2">
      <div>Total: {totalCount}</div>
      <div className="flex gap-2">
        <Button intent="icon" aria-label="Previous page" onClick={previous} disabled={!hasPrevious}>
          <ChevronLeft />
        </Button>
        <Button intent="icon" aria-label="Next page" onClick={next} disabled={!hasNext}>
          <ChevronRight />
        </Button>
      </div>
    </div>
  );
};

export { Pagination };
