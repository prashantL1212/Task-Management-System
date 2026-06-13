import { TASK_STATUS_LABELS } from '../utils/taskEnums';

// Renders a task status integer as a colored label.
export default function StatusBadge({ status }) {
  return (
    <span className={`badge badge--status-${status}`}>
      {TASK_STATUS_LABELS[status] ?? 'Unknown'}
    </span>
  );
}
