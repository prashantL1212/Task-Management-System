using Microsoft.EntityFrameworkCore;
using TMS.Domain.Common;
using TMS.Domain.Entities;

namespace TMS.Infrastructure.Persistence;

/// <summary>
/// EF Core database context. Exposes the Tasks and Users tables, applies all
/// entity configurations from this assembly, and stamps audit timestamps on save.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        ApplyAuditTimestamps();
        return base.SaveChanges();
    }

    /// <summary>
    /// Sets <c>CreatedDate</c> on insert (unless already provided, e.g. by the
    /// seeder) and <c>ModifiedDate</c> on every task update — including soft deletes.
    /// </summary>
    private void ApplyAuditTimestamps()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added && entry.Entity.CreatedDate == default)
                entry.Entity.CreatedDate = now;
        }

        foreach (var entry in ChangeTracker.Entries<TaskItem>())
        {
            if (entry.State == EntityState.Modified)
                entry.Entity.ModifiedDate = now;
        }
    }
}
