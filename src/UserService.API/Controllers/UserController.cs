using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.API.Domain.Contracts;
using UserService.API.Services;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/users")]
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
        [HttpPost("login")]
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
        [HttpPost("register")]
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
        [HttpGet("current")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrentUserResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var response = await _userService.GetCurrentUserAsync();
            return Ok(response);
        }



        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>An <see cref="IActionResult"/> containing the user response.</returns>
        [HttpDelete("{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteUserAsync(int userId)
        {
            var response = await _userService.DeleteUserAsync(userId);
            return Ok(response);
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing a list of user responses.</returns>
        [HttpGet("all-users")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserResponse>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetAllUser()
        {
            var response = await _userService.GetAllUser();
            return Ok(response);
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the user response.</returns>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> LogoutAsync()
        {
            var response = await _userService.LogoutAsync();
            return Ok(response);
        }

        /// <summary>
        /// Refreshes the authentication token.
        /// </summary>
        /// <param name="refreshTokenRequest">The refresh token request.</param>
        /// <returns>An <see cref="IActionResult"/> containing the user response.</returns>
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var response = await _userService.RefreshTokenAsync(refreshTokenRequest);
            return Ok(response);
        }

        /// <summary>
        /// Revokes the refresh token.
        /// </summary>
        /// <param name="refreshTokenRemoveRequest">The refresh token removal request.</param>
        /// <returns>An <see cref="IActionResult"/> indicating whether the token was successfully revoked.</returns>
        [HttpPost("revoke-token")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest refreshTokenRemoveRequest)
        {
            var response = await _userService.RevokeRefreshToken(refreshTokenRemoveRequest);
            return Ok(response);
        }


    }
}
