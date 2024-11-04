using Microsoft.EntityFrameworkCore;
using UserService.API.Domain.Entities;

namespace UserService.API.Infrastructure.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                 new Role
                 {
                     Id = "userdidhere",
                     Name = "Admin",
                     NormalizedName = "ADMIN"
                 },
                 new Role
                 {
                     Id = "Useridherso",
                     Name = "User",
                     NormalizedName = "USER"
                 }
             );
        }
    }
}
