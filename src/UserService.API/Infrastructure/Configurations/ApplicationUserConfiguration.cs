using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.API.Domain.Entities;

namespace UserService.API.Infrastructure.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(p => p.Role).HasMaxLength(50);
            builder.Property(p => p.ProfilePicture).HasMaxLength(255);

            builder.HasData(
                new ApplicationUser
                {
                    Id = "f3a8ec7c-ab34-4c89-a71b-fcbf9283f8e1",
                    UserName = "clifford",
                    Email = "seeduser@example.com",
                    Role = "Backend developer",
                    ProfilePicture = "https://randomuser.me/api/portraits/men/1.jpg"
                }
            );
        }
    }
}
