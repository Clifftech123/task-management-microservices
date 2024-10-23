using Microsoft.AspNetCore.Identity;

namespace UserService.API.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? Role { get; set; }
        public string ? ProfilePicture { get; set; }
        public string? Token { get; set; }




    }
}
