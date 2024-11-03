using Microsoft.AspNetCore.Identity;
using UserService.API.Domain.Contracts;

namespace UserService.API.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public UserRole Role { get; set; }
        public string? ProfilePicture { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
