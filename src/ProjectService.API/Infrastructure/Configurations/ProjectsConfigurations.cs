using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectService.API.Domain.Entities;

namespace ProjectService.API.Infrastructure.Configurations
{
    public class ProjectsConfigurations : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            // Define the primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();

            var projects = new List<Project>
            {
                new Project { Id = 1, Name = "Project 1", Description = "Description 1",
                    StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(10), UserId = "user1",
                    CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow },
                new Project { Id = 2, Name = "Project 2", Description = "Description 2",
                    StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(10), UserId = "user2",
                    CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow },
                new Project { Id = 3, Name = "Project 3", Description = "Description 3",
                    StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(10), UserId = "user3",
                    CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow },
            };

            builder.HasData(projects);

            builder.HasMany(p => p.Tags)
                .WithMany(t => t.Project)
                .UsingEntity<Dictionary<string, object>>(
                    "ProjectTags",
                    j => j.HasOne<Tags>().WithMany().HasForeignKey("TagId"),
                    j => j.HasOne<Project>().WithMany().HasForeignKey("ProjectId")
                );
        }
    }
}
