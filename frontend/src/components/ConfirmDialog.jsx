import Modal from './Modal';

// Generic confirmation dialog. Used for delete, but reusable for any confirm action.
export default function ConfirmDialog({
  title,
  message,
  confirmLabel = 'Confirm',
  loading = false,
  error = '',
  onConfirm,
  onCancel,
}) {
  return (
    <Modal title={title} onClose={onCancel}>
      <div className="stack">
        {error && (
          <div className="alert alert--error" role="alert">
            <p className="alert__message">{error}</p>
          </div>
        )}

        <p className="page__text">{message}</p>

        <div className="task-form__actions">
          <button type="button" className="btn btn--ghost" onClick={onCancel} disabled={loading}>
            Cancel
          </button>
          <button type="button" className="btn btn--danger" onClick={onConfirm} disabled={loading}>
            {loading ? 'Working…' : confirmLabel}
          </button>
        </div>
      </div>
    </Modal>
  );
}
