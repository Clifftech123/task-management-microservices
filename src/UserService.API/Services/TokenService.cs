using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration settings.</param>
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            var jwtSettings = _configuration.GetSection("JwtSettings");
            _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["key"]));
            _validIssuer = jwtSettings["validIssuer"];
            _validAudience = jwtSettings["validAudience"];
            _expires = Convert.ToDouble(jwtSettings["expires"]);
        }

        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <returns>A JWT token as a string.</returns>
        public async Task<string> GenerateJwtToken(IdentityUser user)
        {
            var signingCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);
            var claims = await GetClaimsAsync(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

      

        /// <summary>
        /// Gets the claims for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to get the claims.</param>
        /// <returns>A list of claims.</returns>
        private async Task<List<Claim>> GetClaimsAsync(IdentityUser user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Name, user?.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
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
