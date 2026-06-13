using Microsoft.EntityFrameworkCore;
using TMS.Application.Interfaces.Security;
using TMS.Domain.Entities;
using TMS.Domain.Enums;
using TMS.Infrastructure.Persistence;
using TaskStatus = TMS.Domain.Enums.TaskStatus;

namespace TMS.Infrastructure.Seed;

/// <summary>
/// Idempotent seeder for development use only. Intended to be invoked from the
/// API startup when <c>IsDevelopment()</c> is true. Inserts a default admin user
/// and sample tasks, but only when the respective tables are empty.
/// </summary>
public static class DevelopmentDataSeeder
{
    public static async Task SeedAsync(
        ApplicationDbContext context,
        IPasswordHasher passwordHasher,
        CancellationToken cancellationToken = default)
    {
        if (!await context.Users.AnyAsync(cancellationToken))
        {
            context.Users.Add(new User
            {
                Username = "admin",
                PasswordHash = passwordHasher.Hash("Admin@123"),
                Role = "Admin"
            });
            await context.SaveChangesAsync(cancellationToken);
        }

        if (!await context.Tasks.AnyAsync(cancellationToken))
        {
            context.Tasks.AddRange(BuildSampleTasks());
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    private static IEnumerable<TaskItem> BuildSampleTasks() => new List<TaskItem>
    {
        new() { Title = "Set up project repository", Description = "Initialize git repo, branches and CI pipeline.", Status = TaskStatus.Done, Priority = TaskPriority.High, AssignedTo = "Alice" },
        new() { Title = "Design database schema", Description = "Model Tasks and Users tables.", Status = TaskStatus.Done, Priority = TaskPriority.High, AssignedTo = "Bob" },
        new() { Title = "Implement authentication", Description = "JWT login and protected endpoints.", Status = TaskStatus.InProgress, Priority = TaskPriority.Critical, AssignedTo = "Carol" },
        new() { Title = "Build task CRUD API", Description = "List, get, create, update and soft delete.", Status = TaskStatus.InProgress, Priority = TaskPriority.High, AssignedTo = "Dave" },
        new() { Title = "Add request validation", Description = "FluentValidation for create and update.", Status = TaskStatus.ToDo, Priority = TaskPriority.Medium, AssignedTo = "Erin" },
        new() { Title = "Create summary endpoint", Description = "Raw SQL counts grouped by status and priority.", Status = TaskStatus.ToDo, Priority = TaskPriority.Medium, AssignedTo = "Frank" },
        new() { Title = "Set up React app", Description = "Vite project, routing and Axios client.", Status = TaskStatus.InProgress, Priority = TaskPriority.Medium, AssignedTo = "Grace" },
        new() { Title = "Implement login page", Description = "Form, token storage and protected routes.", Status = TaskStatus.ToDo, Priority = TaskPriority.High, AssignedTo = "Heidi" },
        new() { Title = "Build task dashboard", Description = "Listing, filters and priority indicators.", Status = TaskStatus.ToDo, Priority = TaskPriority.Medium, AssignedTo = "Ivan" },
        new() { Title = "Write unit tests", Description = "Cover service layer and validators.", Status = TaskStatus.ToDo, Priority = TaskPriority.Low, AssignedTo = "Judy" },
        new() { Title = "Add global error handling", Description = "Consistent API error responses.", Status = TaskStatus.ToDo, Priority = TaskPriority.Medium, AssignedTo = "Mallory" },
        new() { Title = "Containerize the stack", Description = "Dockerfiles and docker-compose for all services.", Status = TaskStatus.ToDo, Priority = TaskPriority.Low, AssignedTo = "Niaj" }
    };
}
