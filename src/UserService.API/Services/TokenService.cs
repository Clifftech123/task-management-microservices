using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserService.API.Domain.Entities;

namespace UserService.API.Services
{
    /// <summary>
    /// Service for generating JWT tokens.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _secretKey;
        private readonly string _validIssuer;
        private readonly string _validAudience;
        private readonly double _expires;
        private readonly ILogger<TokenService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration settings.</param>
        /// <param name="logger">The logger instance.</param>
        public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = jwtSettings["key"] ?? throw new ArgumentNullException("JwtSettings:key");
            _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            _validIssuer = jwtSettings["validIssuer"] ?? throw new ArgumentNullException("JwtSettings:validIssuer");
            _validAudience = jwtSettings["validAudience"] ?? throw new ArgumentNullException("JwtSettings:validAudience");
            _expires = Convert.ToDouble(jwtSettings["expires"] ?? throw new ArgumentNullException("JwtSettings:expires"));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <returns>A JWT token as a string.</returns>
        public async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var signingCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);
            var claims = await GetClaimsAsync(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            _logger.LogInformation("Token generated for user {UserName}", user.UserName);
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
        public Task<bool> ValidateRefreshToken(ApplicationUser user, string refreshToken)
        {
            if (user.RefreshToken == refreshToken && user.RefreshTokenExpiryTime > DateTime.Now)
            {
                _logger.LogInformation("Refresh token validated for user {UserName}", user.UserName);
                return Task.FromResult(true);
            }
            _logger.LogInformation("Refresh token validation failed for user {UserName}", user.UserName);
            return Task.FromResult(false);
        }

        /// <summary>
        /// Gets the claims for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to get the claims.</param>
        /// <returns>A list of claims.</returns>
        private Task<List<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user?.UserName ?? throw new ArgumentNullException(nameof(user.UserName))),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                };
                return Task.FromResult(claims);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting claims");
                throw new InvalidOperationException("Error getting claims", ex);
            }
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
