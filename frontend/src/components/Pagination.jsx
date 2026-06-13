// Page navigation: Prev / numbered pages / Next. Renders nothing for a single page.
export default function Pagination({ page, totalPages, onPageChange }) {
  if (totalPages <= 1) {
    return null;
  }

  const pages = Array.from({ length: totalPages }, (_, index) => index + 1);

  return (
    <nav className="pagination" aria-label="Task pagination">
      <button
        type="button"
        className="btn btn--sm btn--ghost"
        onClick={() => onPageChange(page - 1)}
        disabled={page === 1}
      >
        Prev
      </button>

      <ul className="pagination__pages">
        {pages.map((pageNumber) => (
          <li key={pageNumber}>
            <button
              type="button"
              className={`pagination__page${pageNumber === page ? ' pagination__page--active' : ''}`}
              onClick={() => onPageChange(pageNumber)}
              aria-current={pageNumber === page ? 'page' : undefined}
            >
              {pageNumber}
            </button>
          </li>
        ))}
      </ul>

      <button
        type="button"
        className="btn btn--sm btn--ghost"
        onClick={() => onPageChange(page + 1)}
        disabled={page === totalPages}
      >
        Next
      </button>
    </nav>
  );
}
