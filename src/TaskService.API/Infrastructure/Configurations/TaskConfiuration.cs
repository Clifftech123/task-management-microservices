using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.API.Domain.Entities;

namespace TaskService.API.Infrastructure.Configurations
{
    public class TaskConfiuration : IEntityTypeConfiguration<TaskEntities>
    {

        public void Configure(EntityTypeBuilder<TaskEntities> builder)
        {

        }
    }
}
