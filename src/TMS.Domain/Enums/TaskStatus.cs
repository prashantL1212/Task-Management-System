namespace TMS.Domain.Enums;

/// <summary>
/// Workflow state of a task. Stored as <see cref="int"/> in the database.
/// </summary>
public enum TaskStatus
{
    ToDo = 0,
    InProgress = 1,
    Done = 2
}
