using TMS.Domain.Common;
using TMS.Domain.Enums;
// Disambiguate from System.Threading.Tasks.TaskStatus (pulled in by ImplicitUsings).
using TaskStatus = TMS.Domain.Enums.TaskStatus;

namespace TMS.Domain.Entities;

/// <summary>
/// A work item that can be created, assigned, tracked, updated and soft-deleted.
/// </summary>
public class TaskItem : BaseEntity
{
    /// <summary>Short title of the task. Required.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Optional longer description.</summary>
    public string? Description { get; set; }

    /// <summary>Current workflow status. Defaults to <see cref="TaskStatus.ToDo"/>.</summary>
    public TaskStatus Status { get; set; } = TaskStatus.ToDo;

    /// <summary>Priority level. Defaults to <see cref="TaskPriority.Medium"/>.</summary>
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    /// <summary>
    /// Free-text name of the assignee. Intentionally a plain string with no
    /// relationship to <see cref="User"/> — the Users table is auth-only.
    /// </summary>
    public string? AssignedTo { get; set; }

    /// <summary>Soft-delete flag. Deleted tasks are retained for auditability.</summary>
    public bool IsDeleted { get; set; }

    /// <summary>UTC timestamp of the last update; null until first modified.</summary>
    public DateTime? ModifiedDate { get; set; }
}
