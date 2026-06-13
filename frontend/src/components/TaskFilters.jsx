import { TASK_STATUS_LABELS, TASK_PRIORITY_LABELS } from '../utils/taskEnums';

// Status + priority filter bar. Values are strings ('' = All) so they bind
// cleanly to <select>; the parent converts to numbers for the API call.
export default function TaskFilters({
  status,
  priority,
  onStatusChange,
  onPriorityChange,
  onClear,
  disabled = false,
}) {
  const hasActiveFilters = status !== '' || priority !== '';

  return (
    <div className="filters">
      <label className="filters__field">
        <span className="filters__label">Status</span>
        <select
          className="field__input"
          value={status}
          onChange={(event) => onStatusChange(event.target.value)}
          disabled={disabled}
        >
          <option value="">All statuses</option>
          {Object.entries(TASK_STATUS_LABELS).map(([value, label]) => (
            <option key={value} value={value}>
              {label}
            </option>
          ))}
        </select>
      </label>

      <label className="filters__field">
        <span className="filters__label">Priority</span>
        <select
          className="field__input"
          value={priority}
          onChange={(event) => onPriorityChange(event.target.value)}
          disabled={disabled}
        >
          <option value="">All priorities</option>
          {Object.entries(TASK_PRIORITY_LABELS).map(([value, label]) => (
            <option key={value} value={value}>
              {label}
            </option>
          ))}
        </select>
      </label>

      {hasActiveFilters && (
        <button
          type="button"
          className="btn btn--ghost btn--sm filters__clear"
          onClick={onClear}
          disabled={disabled}
        >
          Clear filters
        </button>
      )}
    </div>
  );
}
