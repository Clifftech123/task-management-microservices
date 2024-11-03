using Microsoft.AspNetCore.Mvc;
using ProjectService.API.Services;

namespace ProjectService.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class CurrentUserController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;

        public CurrentUserController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Gets the current user ID.
        /// </summary>
        /// <returns>An IActionResult containing the current user ID.</returns>
        [HttpGet("current")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userId = _currentUserService.GetUserId();
                return Ok(new { message = "Current user successfully fetched", userId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the current user", error = ex.Message });

            }
        }
    }
}

