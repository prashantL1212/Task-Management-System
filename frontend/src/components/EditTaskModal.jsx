import { useState } from 'react';
import Modal from './Modal';
import { updateTask } from '../api/taskApi';
import { TASK_STATUS_LABELS, TASK_PRIORITY_LABELS } from '../utils/taskEnums';

// Edit modal. Pre-fills a controlled form from the selected task and PUTs the
// changes. Unlike create, status IS editable here.
export default function EditTaskModal({ task, onClose, onSuccess }) {
  const [form, setForm] = useState({
    title: task.title ?? '',
    description: task.description ?? '',
    status: task.status,
    priority: task.priority,
    assignedTo: task.assignedTo ?? '',
  });
  const [errors, setErrors] = useState({});
  const [submitting, setSubmitting] = useState(false);
  const [submitError, setSubmitError] = useState('');

  const handleChange = (event) => {
    const { name, value } = event.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const validate = () => {
    const next = {};
    const title = form.title.trim();

    if (!title) {
      next.title = 'Title is required.';
    } else if (title.length > 200) {
      next.title = 'Title must not exceed 200 characters.';
    }
    if (form.description.length > 1000) {
      next.description = 'Description must not exceed 1000 characters.';
    }
    if (form.assignedTo.length > 100) {
      next.assignedTo = 'Assigned To must not exceed 100 characters.';
    }
    return next;
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setSubmitError('');

    const validationErrors = validate();
    setErrors(validationErrors);
    if (Object.keys(validationErrors).length > 0) {
      return;
    }

    setSubmitting(true);
    try {
      const payload = {
        title: form.title.trim(),
        description: form.description.trim() || null,
        status: Number(form.status),
        priority: Number(form.priority),
        assignedTo: form.assignedTo.trim() || null,
      };

      const response = await updateTask(task.id, payload);
      onSuccess(response.data);
    } catch (err) {
      const apiResponse = err.response?.data;
      if (Array.isArray(apiResponse?.errors) && apiResponse.errors.length > 0) {
        setSubmitError(apiResponse.errors.join(' '));
      } else {
        setSubmitError(apiResponse?.message || 'Failed to update task.');
      }
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <Modal title={`Edit Task #${task.id}`} onClose={onClose}>
      <form className="stack" onSubmit={handleSubmit} noValidate>
        {submitError && (
          <div className="alert alert--error" role="alert">
            <p className="alert__message">{submitError}</p>
          </div>
        )}

        <label className="field">
          <span className="field__label">Title *</span>
          <input
            className="field__input"
            name="title"
            value={form.title}
            onChange={handleChange}
            disabled={submitting}
          />
          {errors.title && <span className="field__error">{errors.title}</span>}
        </label>

        <label className="field">
          <span className="field__label">Description</span>
          <textarea
            className="field__input field__input--area"
            name="description"
            value={form.description}
            onChange={handleChange}
            disabled={submitting}
            rows={3}
          />
          {errors.description && <span className="field__error">{errors.description}</span>}
        </label>

        <div className="task-form__row">
          <label className="field">
            <span className="field__label">Status</span>
            <select
              className="field__input"
              name="status"
              value={form.status}
              onChange={handleChange}
              disabled={submitting}
            >
              {Object.entries(TASK_STATUS_LABELS).map(([value, label]) => (
                <option key={value} value={value}>
                  {label}
                </option>
              ))}
            </select>
          </label>

          <label className="field">
            <span className="field__label">Priority</span>
            <select
              className="field__input"
              name="priority"
              value={form.priority}
              onChange={handleChange}
              disabled={submitting}
            >
              {Object.entries(TASK_PRIORITY_LABELS).map(([value, label]) => (
                <option key={value} value={value}>
                  {label}
                </option>
              ))}
            </select>
          </label>
        </div>

        <label className="field">
          <span className="field__label">Assigned To</span>
          <input
            className="field__input"
            name="assignedTo"
            value={form.assignedTo}
            onChange={handleChange}
            disabled={submitting}
          />
          {errors.assignedTo && <span className="field__error">{errors.assignedTo}</span>}
        </label>

        <div className="task-form__actions">
          <button type="button" className="btn btn--ghost" onClick={onClose} disabled={submitting}>
            Cancel
          </button>
          <button type="submit" className="btn btn--primary" disabled={submitting}>
            {submitting ? 'Saving…' : 'Save Changes'}
          </button>
        </div>
      </form>
    </Modal>
  );
}
