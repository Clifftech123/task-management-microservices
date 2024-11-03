using System.Security.Claims;

namespace TaskService.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CurrentUserService> _logger;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, ILogger<CurrentUserService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public string GetUserId()
        {
            var GetUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (GetUserId == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }
            _logger.LogInformation($"Current user id: {GetUserId}");
            return GetUserId;
        }
    }
}
