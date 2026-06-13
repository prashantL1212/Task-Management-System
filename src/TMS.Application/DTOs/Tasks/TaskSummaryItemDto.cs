using TMS.Domain.Enums;
using TaskStatus = TMS.Domain.Enums.TaskStatus;

namespace TMS.Application.DTOs.Tasks;

/// <summary>
/// A single grouped row of the task summary: the number of tasks that share a
/// given status and priority. Maps directly to one row of
/// <c>GROUP BY Status, Priority</c>.
/// </summary>
public class TaskSummaryItemDto
{
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public int Count { get; set; }
}
