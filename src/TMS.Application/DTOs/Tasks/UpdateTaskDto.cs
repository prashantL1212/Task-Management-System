using TMS.Domain.Enums;
using TaskStatus = TMS.Domain.Enums.TaskStatus;

namespace TMS.Application.DTOs.Tasks;

/// <summary>
/// Request payload for updating an existing task. The task id comes from the
/// route, not the body. Every member is nullable to support partial updates:
/// a <c>null</c> member means "leave this field unchanged". Validated by
/// <c>UpdateTaskValidator</c>. (See note on clearing nullable string fields.)
/// </summary>
public class UpdateTaskDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TaskStatus? Status { get; set; }
    public TaskPriority? Priority { get; set; }
    public string? AssignedTo { get; set; }
}
