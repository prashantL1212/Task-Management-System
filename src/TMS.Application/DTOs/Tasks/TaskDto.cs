using TMS.Domain.Enums;
// Disambiguate from System.Threading.Tasks.TaskStatus (pulled in by ImplicitUsings).
using TaskStatus = TMS.Domain.Enums.TaskStatus;

namespace TMS.Application.DTOs.Tasks;

/// <summary>
/// Read model returned to clients for a single task. Deliberately omits the
/// internal <c>IsDeleted</c> flag — soft-deleted tasks are never returned.
/// </summary>
public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
