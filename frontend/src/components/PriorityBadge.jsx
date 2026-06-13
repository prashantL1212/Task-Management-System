import { TASK_PRIORITY_LABELS } from '../utils/taskEnums';

// Renders a task priority integer as a colored label.
export default function PriorityBadge({ priority }) {
  return (
    <span className={`badge badge--priority-${priority}`}>
      {TASK_PRIORITY_LABELS[priority] ?? 'Unknown'}
    </span>
  );
}
