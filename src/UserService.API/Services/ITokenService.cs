using Microsoft.AspNetCore.Identity;

namespace UserService.API.Services
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(IdentityUser user);
    }
}
