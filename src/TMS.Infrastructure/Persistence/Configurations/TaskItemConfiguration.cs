using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Entities;

namespace TMS.Infrastructure.Persistence.Configurations;

/// <summary>Fluent EF Core mapping for <see cref="TaskItem"/>.</summary>
public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("Tasks");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Description)
            .HasMaxLength(1000);

        builder.Property(t => t.AssignedTo)
            .HasMaxLength(100);

        // Store enums as their underlying int values.
        builder.Property(t => t.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(t => t.Priority)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(t => t.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(t => t.CreatedDate)
            .IsRequired();

        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.Priority);

        // Global soft-delete filter: deleted tasks are excluded from every query.
        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}
