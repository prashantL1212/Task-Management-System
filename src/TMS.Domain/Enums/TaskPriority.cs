namespace TMS.Domain.Enums;

/// <summary>
/// Importance level of a task. Stored as <see cref="int"/> in the database.
/// </summary>
public enum TaskPriority
{
    Low = 0,
    Medium = 1,
    High = 2,
    Critical = 3
}
