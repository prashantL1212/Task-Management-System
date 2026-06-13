using TMS.Application.DTOs.Tasks;
using TMS.Domain.Entities;
using TMS.Domain.Enums;
using TaskStatus = TMS.Domain.Enums.TaskStatus;

namespace TMS.Application.Interfaces.Repositories;

/// <summary>
/// Persistence contract for tasks. Implemented in the Infrastructure layer over
/// EF Core. Only non-deleted tasks are returned (soft-delete query filter).
/// </summary>
public interface ITaskRepository
{
    /// <summary>Returns tasks, optionally filtered by status and/or priority.</summary>
    Task<IReadOnlyList<TaskItem>> GetAllAsync(
        TaskStatus? status,
        TaskPriority? priority,
        CancellationToken cancellationToken = default);

    /// <summary>Returns a single task by id, or null if not found / deleted.</summary>
    Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Stages a new task for insertion.</summary>
    Task AddAsync(TaskItem task, CancellationToken cancellationToken = default);

    /// <summary>Marks an existing task as modified.</summary>
    void Update(TaskItem task);

    /// <summary>
    /// Returns aggregated counts grouped by status and priority. Implemented
    /// with a raw SQL query, as required by the specification.
    /// </summary>
    Task<TaskSummaryDto> GetSummaryAsync(CancellationToken cancellationToken = default);

    /// <summary>Persists all pending changes; returns affected row count.</summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
