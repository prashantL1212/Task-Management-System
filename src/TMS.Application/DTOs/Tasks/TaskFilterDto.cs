using TMS.Domain.Enums;
using TaskStatus = TMS.Domain.Enums.TaskStatus;

namespace TMS.Application.DTOs.Tasks;

/// <summary>
/// Optional query filters for listing tasks. Null members mean "no filter".
/// Bound from the query string (e.g. <c>?status=1&amp;priority=2</c>).
/// </summary>
public class TaskFilterDto
{
    public TaskStatus? Status { get; set; }
    public TaskPriority? Priority { get; set; }
}
