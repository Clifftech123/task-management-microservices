using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserService.API.Domain.Entities;

namespace UserService.API.Services
{
    /// <summary>
    /// Service for generating JWT tokens
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _secretKey;
        private readonly string _validIssuer;
        private readonly string _validAudience;
        private readonly double _expires;
        private readonly ILogger<TokenService> _logger;
        private readonly UserManager<User> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration settings.</param>
        /// <param name="logger">The logger instance.</param>
        public TokenService(UserManager<User> userManager, IConfiguration configuration, ILogger<TokenService> logger)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = jwtSettings["key"] ?? throw new ArgumentNullException(nameof(jwtSettings) + ":key");
            _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            _validIssuer = jwtSettings["validIssuer"] ?? throw new ArgumentNullException(nameof(jwtSettings) + ":validIssuer");
            _validAudience = jwtSettings["validAudience"] ?? throw new ArgumentNullException(nameof(jwtSettings) + ":validAudience");
            _expires = Convert.ToDouble(jwtSettings["expires"] ?? throw new ArgumentNullException(nameof(jwtSettings) + ":expires"));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <returns>A JWT token as a string.</returns>
        public async Task<string> GenerateJwtToken(User user)
        {
            if (user == null)
            {
                _logger.LogError("GenerateJwtToken: User is null");
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            var signingCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);
            var claims = await GetClaimsAsync(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            var role = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            _logger.LogInformation("Token generated for user {UserName} with role {Role}", user.UserName, role);
            return tokenHandler;
        }

        /// <summary>
        /// Generates a refresh token.
        /// </summary>
        /// <returns>A refresh token as a string.</returns>
        public Task<string> GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                var refreshToken = Convert.ToBase64String(randomNumber);
                return Task.FromResult(refreshToken);
            }
        }

        /// <summary>
        /// Validates the refresh token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to validate the token.</param>
        /// <param name="refreshToken">The refresh token to validate.</param>
        /// <returns>A boolean indicating whether the token is valid.</returns>
        public Task<bool> ValidateRefreshToken(User user, string refreshToken)
        {
            if (user == null)
            {
                _logger.LogError("ValidateRefreshToken: User is null");
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            if (user.RefreshToken == refreshToken && user.RefreshTokenExpiryTime > DateTime.Now)
            {
                _logger.LogInformation("Refresh token validated for user {UserName}", user.UserName);
                return Task.FromResult(true);
            }
            _logger.LogInformation("Refresh token validation failed for user {UserName}", user.UserName);
            return Task.FromResult(false);
        }

        /// <summary>
        ///  Adds the claims to the token for the specified user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<List<Claim>> GetClaimsAsync(User user)
        {
            if (user == null)
            {
                _logger.LogError("GetClaimsAsync: User is null");
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id),
            };

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            return claims;
        }

        /// <summary>
        /// Generates the token options.
        /// </summary>
        /// <param name="signingCredentials">The signing credentials.</param>
        /// <param name="claims">The claims to include in the token.</param>
        /// <returns>A <see cref="JwtSecurityToken"/> instance.</returns>
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            return new JwtSecurityToken(
                issuer: _validIssuer,
                audience: _validAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_expires),
                signingCredentials: signingCredentials
            );
        }
    }
}
