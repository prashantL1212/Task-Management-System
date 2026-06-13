import { TASK_STATUS } from '../utils/taskEnums';

// Sums the grouped summary counts for a given status.
// summary.groups = [{ status, priority, count }, ...]
function countByStatus(groups, status) {
  return groups
    .filter((group) => group.status === status)
    .reduce((total, group) => total + group.count, 0);
}

export default function SummaryCards({ summary }) {
  const groups = summary?.groups ?? [];

  const cards = [
    { label: 'Total Tasks', value: summary?.total ?? 0, variant: 'total' },
    { label: 'To Do', value: countByStatus(groups, TASK_STATUS.TODO), variant: 'todo' },
    { label: 'In Progress', value: countByStatus(groups, TASK_STATUS.IN_PROGRESS), variant: 'progress' },
    { label: 'Completed', value: countByStatus(groups, TASK_STATUS.DONE), variant: 'done' },
  ];

  return (
    <div className="cards">
      {cards.map((card) => (
        <div className={`card card--${card.variant}`} key={card.label}>
          <span className="card__value">{card.value}</span>
          <span className="card__label">{card.label}</span>
        </div>
      ))}
    </div>
  );
}
