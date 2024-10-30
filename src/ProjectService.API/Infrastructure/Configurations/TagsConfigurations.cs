using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectService.API.Domain.Entities;

namespace ProjectService.API.Infrastructure.Configurations
{
    public class TagsConfigurations : IEntityTypeConfiguration<Tags>
    {
        public void Configure(EntityTypeBuilder<Tags> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.UserId).IsRequired();

            builder.HasMany(x => x.Project)
                .WithMany(x => x.Tags)
                .UsingEntity<Dictionary<string, object>>(
                    "ProjectTag",
                    j => j.HasOne<Project>().WithMany().HasForeignKey("ProjectId"),
                    j => j.HasOne<Tags>().WithMany().HasForeignKey("TagId"));

            builder.HasData(
                new Tags { Id = 1, Name = "C#", UserId = "user1" },
                new Tags { Id = 2, Name = "ASP.NET", UserId = "user1" },
                new Tags { Id = 3, Name = "Entity Framework", UserId = "user2" },
                new Tags { Id = 4, Name = "Azure", UserId = "user2" },
                new Tags { Id = 5, Name = "Blazor", UserId = "user3" },
                new Tags { Id = 6, Name = "Microservices", UserId = "user3" },
                new Tags { Id = 7, Name = "Docker", UserId = "user4" },
                new Tags { Id = 8, Name = "Kubernetes", UserId = "user4" },
                new Tags { Id = 9, Name = "DevOps", UserId = "user5" },
                new Tags { Id = 10, Name = "CI/CD", UserId = "user5" }
            );
        }
    }
}
