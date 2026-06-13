import { useState, useEffect, useMemo } from 'react';

// Client-side pagination over an in-memory list. Returns the current page slice
// plus controls. Page auto-clamps when the list shrinks (e.g. after a delete or
// filter), so you never end up stranded on a now-empty page.
export function usePagination(items, pageSize) {
  const [page, setPage] = useState(1);

  const totalPages = Math.max(1, Math.ceil(items.length / pageSize));

  useEffect(() => {
    if (page > totalPages) {
      setPage(totalPages);
    }
  }, [page, totalPages]);

  const pageItems = useMemo(() => {
    const start = (page - 1) * pageSize;
    return items.slice(start, start + pageSize);
  }, [items, page, pageSize]);

  return { page, setPage, totalPages, pageItems };
}
