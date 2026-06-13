import { useState, useMemo, useEffect } from 'react';
import { useDashboardData } from '../hooks/useDashboardData';
import { usePagination } from '../hooks/usePagination';
import { deleteTask } from '../api/taskApi';
import { TASKS_PER_PAGE } from '../utils/constants';
import SummaryCards from '../components/SummaryCards';
import TaskFilters from '../components/TaskFilters';
import TaskTable from '../components/TaskTable';
import TaskForm from '../components/TaskForm';
import EditTaskModal from '../components/EditTaskModal';
import ConfirmDialog from '../components/ConfirmDialog';
import Pagination from '../components/Pagination';

export default function DashboardPage() {
  // Filters: '' = no filter. Kept as strings for the <select> bindings.
  const [statusFilter, setStatusFilter] = useState('');
  const [priorityFilter, setPriorityFilter] = useState('');

  // Stable filters object for the data hook (only changes when a filter changes).
  const filters = useMemo(() => {
    const next = {};
    if (statusFilter !== '') next.status = Number(statusFilter);
    if (priorityFilter !== '') next.priority = Number(priorityFilter);
    return next;
  }, [statusFilter, priorityFilter]);

  const { tasks, summary, loading, error, reload } = useDashboardData(filters);

  const [showForm, setShowForm] = useState(false);
  const [successMessage, setSuccessMessage] = useState('');
  const [editingTask, setEditingTask] = useState(null);
  const [deletingTask, setDeletingTask] = useState(null);
  const [deleteLoading, setDeleteLoading] = useState(false);
  const [deleteError, setDeleteError] = useState('');

  // Pagination over the (filtered) tasks, 7 per page.
  const { page, setPage, totalPages, pageItems } = usePagination(tasks, TASKS_PER_PAGE);

  // Reset to page 1 whenever the filters change.
  useEffect(() => {
    setPage(1);
  }, [statusFilter, priorityFilter, setPage]);

  const rangeStart = tasks.length === 0 ? 0 : (page - 1) * TASKS_PER_PAGE + 1;
  const rangeEnd = Math.min(page * TASKS_PER_PAGE, tasks.length);

  const toggleForm = () => {
    setSuccessMessage('');
    setShowForm((open) => !open);
  };

  const handleCreated = (task) => {
    setShowForm(false);
    setSuccessMessage(`Task "${task.title}" created.`);
    setPage(1);
    reload();
  };

  const handleUpdated = (task) => {
    setEditingTask(null);
    setSuccessMessage(`Task "${task.title}" updated.`);
    reload();
  };

  const openDelete = (task) => {
    setDeleteError('');
    setDeletingTask(task);
  };

  const confirmDelete = async () => {
    setDeleteLoading(true);
    setDeleteError('');
    try {
      await deleteTask(deletingTask.id);
      const title = deletingTask.title;
      setDeletingTask(null);
      setSuccessMessage(`Task "${title}" deleted.`);
      reload();
    } catch (err) {
      setDeleteError(err.response?.data?.message || 'Failed to delete task.');
    } finally {
      setDeleteLoading(false);
    }
  };

  const clearFilters = () => {
    setStatusFilter('');
    setPriorityFilter('');
  };

  const initialLoading = loading && summary === null;

  return (
    <section className="page">
      <div className="page__header">
        <h2 className="page__title">Dashboard</h2>
        <div className="page__actions">
          <button type="button" className="btn btn--ghost" onClick={reload} disabled={loading}>
            {loading ? 'Refreshing…' : 'Refresh'}
          </button>
          <button type="button" className="btn btn--primary" onClick={toggleForm}>
            {showForm ? 'Close' : 'New Task'}
          </button>
        </div>
      </div>

      {successMessage && (
        <div className="alert alert--success" role="status">
          <p className="alert__message">{successMessage}</p>
        </div>
      )}

      {showForm && <TaskForm onSuccess={handleCreated} onCancel={() => setShowForm(false)} />}

      {error && (
        <div className="alert alert--error" role="alert">
          <p className="alert__message">{error}</p>
        </div>
      )}

      {initialLoading && <p className="page__text">Loading dashboard…</p>}

      {summary !== null && (
        <>
          <SummaryCards summary={summary} />

          <div className="section-header">
            <h3 className="section-title">Tasks</h3>
            {tasks.length > 0 && (
              <span className="section-header__meta">
                Showing {rangeStart}–{rangeEnd} of {tasks.length}
              </span>
            )}
          </div>

          <TaskFilters
            status={statusFilter}
            priority={priorityFilter}
            onStatusChange={setStatusFilter}
            onPriorityChange={setPriorityFilter}
            onClear={clearFilters}
            disabled={loading}
          />

          <TaskTable tasks={pageItems} onEdit={setEditingTask} onDelete={openDelete} />
          <Pagination page={page} totalPages={totalPages} onPageChange={setPage} />
        </>
      )}

      {editingTask && (
        <EditTaskModal
          task={editingTask}
          onClose={() => setEditingTask(null)}
          onSuccess={handleUpdated}
        />
      )}

      {deletingTask && (
        <ConfirmDialog
          title="Delete task"
          message={`Are you sure you want to delete "${deletingTask.title}"? This action soft-deletes the task.`}
          confirmLabel="Delete"
          loading={deleteLoading}
          error={deleteError}
          onConfirm={confirmDelete}
          onCancel={() => setDeletingTask(null)}
        />
      )}
    </section>
  );
}
