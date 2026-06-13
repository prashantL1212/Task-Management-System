using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.DTOs.Tasks;
using TMS.Application.Interfaces.Services;
using TMS.Shared.Models;

namespace TMS.API.Controllers;

/// <summary>Task management endpoints. All require a valid JWT.</summary>
[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService) => _taskService = taskService;

    /// <summary>Lists tasks, optionally filtered by status and/or priority.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] TaskFilterDto filter, CancellationToken cancellationToken)
    {
        var tasks = await _taskService.GetTasksAsync(filter, cancellationToken);
        return Ok(ApiResponse.Success(tasks));
    }

    /// <summary>Returns a single task by id.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var task = await _taskService.GetTaskByIdAsync(id, cancellationToken);

        if (task is null)
            return NotFound(ApiResponse.Failure<TaskDto>($"Task {id} was not found."));

        return Ok(ApiResponse.Success(task));
    }

    /// <summary>Creates a new task (always starts as ToDo).</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto request, CancellationToken cancellationToken)
    {
        var created = await _taskService.CreateTaskAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            ApiResponse.Success(created, "Task created."));
    }

    /// <summary>Partially updates an existing task.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto request, CancellationToken cancellationToken)
    {
        var updated = await _taskService.UpdateTaskAsync(id, request, cancellationToken);

        if (updated is null)
            return NotFound(ApiResponse.Failure<TaskDto>($"Task {id} was not found."));

        return Ok(ApiResponse.Success(updated, "Task updated."));
    }

    /// <summary>Soft-deletes a task.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _taskService.DeleteTaskAsync(id, cancellationToken);

        if (!deleted)
            return NotFound(ApiResponse.Failure<object>($"Task {id} was not found."));

        return Ok(ApiResponse.Success<object?>(null, "Task deleted."));
    }

    /// <summary>Returns task counts grouped by status and priority.</summary>
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(CancellationToken cancellationToken)
    {
        var summary = await _taskService.GetSummaryAsync(cancellationToken);
        return Ok(ApiResponse.Success(summary));
    }
}
