using System.Security.Claims;

namespace ProjectService.API.Services
{
    /// <summary>
    ///  service to retrieve the current user ID.
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CurrentUserService> _logger;

        /// <summary>
        ///  Retrieves the current user ID.
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="logger"></param>
        public CurrentUserService(IHttpContextAccessor httpContextAccessor, ILogger<CurrentUserService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <summary>
        ///  method to get the user id.
        /// </summary>
        /// <returns></returns>
        public string? GetUserId()
        {
            _logger.LogInformation("Getting user id");
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        }
    }
}
