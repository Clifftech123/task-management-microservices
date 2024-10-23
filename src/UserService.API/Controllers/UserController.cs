using Microsoft.AspNetCore.Mvc;
using UserService.API.Domain.Contracts;
using UserService.API.Services;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="loginRequest">The login request.</param>
        /// <returns>An <see cref="IActionResult"/> containing the user response.</returns>
        [HttpPost("users/login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserRequest loginRequest)
        {
            var response = await _userService.LoginAsync(loginRequest);
            return Ok(response);
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registerRequest">The registration request.</param>
        /// <returns>An <see cref="IActionResult"/> containing the user response.</returns>
        [HttpPost("users/register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterRequest registerRequest)
        {
            var response = await _userService.RegisterAsync(registerRequest);
            return Ok(response);
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the current user response.</returns>
        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrentUserResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var response = await _userService.GetCurrentUserAsync();
            return Ok(response);
        }

        /// <summary>
        /// Updates the current user.
        /// </summary>
        /// <param name="updateCurrentUserRequest">The update request.</param>
        /// <returns>An <see cref="IActionResult"/> containing the user response.</returns>
        [HttpPut("user/update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateCurrentUserAsync([FromBody] UpdateUserRequest updateCurrentUserRequest)
        {
            var response = await _userService.UpdateCurrentUserAsync(updateCurrentUserRequest);
            return Ok(response);
        }
    }
}
