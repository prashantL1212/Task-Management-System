using TMS.Application.DTOs.Tasks;
using TMS.Application.Interfaces.Repositories;
using TMS.Application.Interfaces.Services;
using TMS.Domain.Entities;
using TaskStatus = TMS.Domain.Enums.TaskStatus;

namespace TMS.Application.Services;

/// <summary>
/// Business logic for tasks. Orchestrates <see cref="ITaskRepository"/> and maps
/// explicitly between entities and DTOs (no AutoMapper).
/// </summary>
public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository) => _taskRepository = taskRepository;

    public async Task<IReadOnlyList<TaskDto>> GetTasksAsync(
        TaskFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        var tasks = await _taskRepository.GetAllAsync(filter.Status, filter.Priority, cancellationToken);
        return tasks.Select(MapToDto).ToList();
    }

    public async Task<TaskDto?> GetTaskByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(id, cancellationToken);
        return task is null ? null : MapToDto(task);
    }

    public async Task<TaskDto> CreateTaskAsync(CreateTaskDto request, CancellationToken cancellationToken = default)
    {
        var task = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            // Initial status is always ToDo — the client cannot control it
            // (CreateTaskDto deliberately has no Status field).
            Status = TaskStatus.ToDo,
            Priority = request.Priority,
            AssignedTo = request.AssignedTo
        };

        await _taskRepository.AddAsync(task, cancellationToken);
        await _taskRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(task);
    }

    public async Task<TaskDto?> UpdateTaskAsync(
        int id,
        UpdateTaskDto request,
        CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(id, cancellationToken);
        if (task is null)
            return null;

        ApplyPartialUpdate(task, request);

        _taskRepository.Update(task);
        await _taskRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(task);
    }

    public async Task<bool> DeleteTaskAsync(int id, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(id, cancellationToken);
        if (task is null)
            return false;

        // Soft delete only — the record is retained for auditability.
        task.IsDeleted = true;
        _taskRepository.Update(task);
        await _taskRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public Task<TaskSummaryDto> GetSummaryAsync(CancellationToken cancellationToken = default)
        => _taskRepository.GetSummaryAsync(cancellationToken);

    // ---------- explicit mapping (no AutoMapper) ----------

    private static TaskDto MapToDto(TaskItem task) => new()
    {
        Id = task.Id,
        Title = task.Title,
        Description = task.Description,
        Status = task.Status,
        Priority = task.Priority,
        AssignedTo = task.AssignedTo,
        CreatedDate = task.CreatedDate,
        ModifiedDate = task.ModifiedDate
    };

    /// <summary>
    /// Copies only the supplied (non-null) fields onto the entity. Null members
    /// of <see cref="UpdateTaskDto"/> leave the existing value unchanged.
    /// </summary>
    private static void ApplyPartialUpdate(TaskItem task, UpdateTaskDto request)
    {
        if (request.Title is not null)
            task.Title = request.Title;

        if (request.Description is not null)
            task.Description = request.Description;

        if (request.Status.HasValue)
            task.Status = request.Status.Value;

        if (request.Priority.HasValue)
            task.Priority = request.Priority.Value;

        if (request.AssignedTo is not null)
            task.AssignedTo = request.AssignedTo;
    }
}
