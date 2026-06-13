namespace TMS.Application.DTOs.Tasks;

/// <summary>
/// Result of the summary endpoint: task counts grouped by status and priority,
/// populated from a single raw SQL query (<c>GROUP BY Status, Priority</c>).
/// Per-status or per-priority totals can be derived by summing <see cref="Groups"/>.
/// </summary>
public class TaskSummaryDto
{
    /// <summary>Total number of (non-deleted) tasks — the sum of all group counts.</summary>
    public int Total { get; set; }

    /// <summary>One entry per (status, priority) combination that has tasks.</summary>
    public IReadOnlyList<TaskSummaryItemDto> Groups { get; set; } = Array.Empty<TaskSummaryItemDto>();
}
