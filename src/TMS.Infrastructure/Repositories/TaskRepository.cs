using Microsoft.EntityFrameworkCore;
using TMS.Application.DTOs.Tasks;
using TMS.Application.Interfaces.Repositories;
using TMS.Domain.Entities;
using TMS.Domain.Enums;
using TMS.Infrastructure.Persistence;
using TaskStatus = TMS.Domain.Enums.TaskStatus;

namespace TMS.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of <see cref="ITaskRepository"/>. All LINQ queries run
/// through the global soft-delete filter, so deleted tasks are never returned.
/// </summary>
public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context) => _context = context;

    public async Task<IReadOnlyList<TaskItem>> GetAllAsync(
        TaskStatus? status,
        TaskPriority? priority,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Tasks.AsNoTracking().AsQueryable();

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        if (priority.HasValue)
            query = query.Where(t => t.Priority == priority.Value);

        return await query
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task AddAsync(TaskItem task, CancellationToken cancellationToken = default)
        => await _context.Tasks.AddAsync(task, cancellationToken);

    public void Update(TaskItem task) => _context.Tasks.Update(task);

    public async Task<TaskSummaryDto> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        // Raw SQL summary grouped by Status and Priority, excluding soft-deleted rows.
        var rows = await _context.Database
            .SqlQueryRaw<TaskSummaryRow>(
                "SELECT [Status] AS Status, [Priority] AS Priority, COUNT(*) AS Count " +
                "FROM [Tasks] " +
                "WHERE [IsDeleted] = 0 " +
                "GROUP BY [Status], [Priority]")
            .ToListAsync(cancellationToken);

        var groups = rows
            .Select(r => new TaskSummaryItemDto
            {
                Status = (TaskStatus)r.Status,
                Priority = (TaskPriority)r.Priority,
                Count = r.Count
            })
            .ToList();

        return new TaskSummaryDto
        {
            Total = groups.Sum(g => g.Count),
            Groups = groups
        };
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}

/// <summary>Untyped projection used to read the raw summary query result.</summary>
internal sealed class TaskSummaryRow
{
    public int Status { get; set; }
    public int Priority { get; set; }
    public int Count { get; set; }
}
