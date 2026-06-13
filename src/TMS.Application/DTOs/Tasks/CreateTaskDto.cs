using TMS.Domain.Enums;

namespace TMS.Application.DTOs.Tasks;

/// <summary>
/// Request payload for creating a task. New tasks are always created with
/// <c>Status = ToDo</c> by the service layer, so status is not accepted here.
/// Validated by <c>CreateTaskValidator</c>.
/// </summary>
public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public string? AssignedTo { get; set; }
}
