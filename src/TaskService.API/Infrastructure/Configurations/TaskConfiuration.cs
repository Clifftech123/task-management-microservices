using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.API.Domain.Entities;

namespace TaskService.API.Infrastructure.Configurations
{
    public class TaskConfiuration : IEntityTypeConfiguration<TaskEntities>
    {
        public void Configure(EntityTypeBuilder<TaskEntities> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.DueDate).IsRequired();
            builder.Property(x => x.IsCompleted).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();
            builder.Property(x => x.ProjectId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            // Seed data
            builder.HasData(
                new TaskEntities
                {
                    Id = 1,
                    Title = "Initial Task",
                    Description = "This is the first task",
                    DueDate = DateTime.UtcNow.AddDays(7),
                    IsCompleted = false,
                    Status = 1,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow,
                    ProjectId = 1,
                    UserId = "user1"
                }
            );
        }
    }
}
