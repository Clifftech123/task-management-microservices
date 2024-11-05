using UserService.API.Domain.Entities;

namespace UserService.API.Services
{
    /// <summary>
    ///  Token service interface
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        ///  Generate JWT token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GenerateJwtToken(ApplicationUser user);
        /// <summary>
        ///  Generate refresh token
        /// </summary>
        /// <returns></returns>
        Task<string> GenerateRefreshToken();
        /// <summary>
        ///  Validate refresh token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<bool> ValidateRefreshToken(ApplicationUser user, string refreshToken);

    }
}
