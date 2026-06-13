import { useState } from 'react';
import { createTask } from '../api/taskApi';
import { TASK_PRIORITY, TASK_PRIORITY_LABELS } from '../utils/taskEnums';

// New tasks always start as "To Do" (enforced by the backend), so status is not
// part of this form. Priority defaults to Medium.
const initialForm = {
  title: '',
  description: '',
  priority: TASK_PRIORITY.MEDIUM,
  assignedTo: '',
};

export default function TaskForm({ onSuccess, onCancel }) {
  const [form, setForm] = useState(initialForm);
  const [errors, setErrors] = useState({});
  const [submitting, setSubmitting] = useState(false);
  const [submitError, setSubmitError] = useState('');

  const handleChange = (event) => {
    const { name, value } = event.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  // Client-side validation mirrors the backend rules for instant feedback.
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
        priority: Number(form.priority),
        assignedTo: form.assignedTo.trim() || null,
      };

      const response = await createTask(payload);
      onSuccess(response.data); // hand the created TaskDto back to the parent
    } catch (err) {
      const apiResponse = err.response?.data;
      // Prefer the backend's validation list, then its message, then a fallback.
      if (Array.isArray(apiResponse?.errors) && apiResponse.errors.length > 0) {
        setSubmitError(apiResponse.errors.join(' '));
      } else {
        setSubmitError(apiResponse?.message || 'Failed to create task.');
      }
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <form className="task-form" onSubmit={handleSubmit} noValidate>
      <h3 className="task-form__title">New Task</h3>

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
          placeholder="Task title"
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
          placeholder="Optional description"
        />
        {errors.description && <span className="field__error">{errors.description}</span>}
      </label>

      <div className="task-form__row">
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

        <label className="field">
          <span className="field__label">Assigned To</span>
          <input
            className="field__input"
            name="assignedTo"
            value={form.assignedTo}
            onChange={handleChange}
            disabled={submitting}
            placeholder="Optional"
          />
          {errors.assignedTo && <span className="field__error">{errors.assignedTo}</span>}
        </label>
      </div>

      <div className="task-form__actions">
        <button type="button" className="btn btn--ghost" onClick={onCancel} disabled={submitting}>
          Cancel
        </button>
        <button type="submit" className="btn btn--primary" disabled={submitting}>
          {submitting ? 'Creating…' : 'Create Task'}
        </button>
      </div>
    </form>
  );
}
