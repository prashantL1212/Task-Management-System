using TMS.Application.DTOs.Tasks;

namespace TMS.Application.Interfaces.Services;

/// <summary>
/// Business operations for tasks. Orchestrates the repository and maps between
/// entities and DTOs. Implementation lives in the Application layer.
/// </summary>
public interface ITaskService
{
    Task<IReadOnlyList<TaskDto>> GetTasksAsync(
        TaskFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<TaskDto?> GetTaskByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<TaskDto> CreateTaskAsync(CreateTaskDto request, CancellationToken cancellationToken = default);

    /// <summary>Updates a task; returns null if it does not exist.</summary>
    Task<TaskDto?> UpdateTaskAsync(int id, UpdateTaskDto request, CancellationToken cancellationToken = default);

    /// <summary>Soft-deletes a task; returns false if it does not exist.</summary>
    Task<bool> DeleteTaskAsync(int id, CancellationToken cancellationToken = default);

    Task<TaskSummaryDto> GetSummaryAsync(CancellationToken cancellationToken = default);
}
