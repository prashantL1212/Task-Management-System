import StatusBadge from './StatusBadge';
import PriorityBadge from './PriorityBadge';

export default function TaskTable({ tasks, onEdit, onDelete }) {
  if (!tasks.length) {
    return <p className="page__text">No tasks found.</p>;
  }

  return (
    <div className="table-wrap">
      <table className="table">
        <thead>
          <tr>
            <th>Title</th>
            <th>Description</th>
            <th>Status</th>
            <th>Priority</th>
            <th>Assigned To</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {tasks.map((task) => (
            <tr key={task.id}>
              <td>{task.title}</td>
              <td className="table__muted">{task.description || '—'}</td>
              <td><StatusBadge status={task.status} /></td>
              <td><PriorityBadge priority={task.priority} /></td>
              <td>{task.assignedTo || '—'}</td>
              <td>
                <div className="table__actions">
                  <button
                    type="button"
                    className="btn btn--sm btn--ghost"
                    onClick={() => onEdit(task)}
                  >
                    Edit
                  </button>
                  <button
                    type="button"
                    className="btn btn--sm btn--danger-ghost"
                    onClick={() => onDelete(task)}
                  >
                    Delete
                  </button>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
