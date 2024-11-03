using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.API.Domain.Contracts;
using UserService.API.Domain.Entities;
using UserService.API.Infrastructure.Context;
using UserService.API.Infrastructure.Exceptions;

namespace UserService.API.Services
{
    /// <summary>
    /// Implementation of the user service interface.
    /// </summary>
    public class UserServiceImple : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserServiceImple> _logger;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserServiceImple"/> class.
        /// </summary>
        /// <param name="tokenService">The token service.</param>
        /// <param name="userManager">The user manager.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="currentUserService">The current user service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="context">The application database context.</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters are null.</exception>
        public UserServiceImple(ITokenService tokenService, UserManager<ApplicationUser> userManager, IMapper mapper, ICurrentUserService currentUserService, ILogger<UserServiceImple> logger, ApplicationDbContext context)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Registers a new user asynchronously.
        /// </summary>
        /// <param name="registerRequest">The user registration request.</param>
        /// <returns>The registered user response.</returns>
        /// <exception cref="ArgumentException">Thrown when the registration request is invalid.</exception>
        /// <exception cref="UserAlreadyExistsException">Thrown when a user with the same email already exists.</exception>
        /// <exception cref="InvalidOperationException">Thrown when user registration fails.</exception>
        public async Task<UserResponse> RegisterAsync(UserRegisterRequest registerRequest)
        {
            _logger.LogInformation("Registering user with email {Email}", registerRequest?.Email);
            if (registerRequest == null)
            {
                throw new ArgumentException("User registration data is missing.");
            }

            if (string.IsNullOrEmpty(registerRequest.Password) || !registerRequest.Password.Any(char.IsDigit))
            {
                _logger.LogWarning("Password does not contain a number.");
                throw new ArgumentException("Password must include at least one number.");
            }

            var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email ?? throw new ArgumentException("Email is required."));
            if (existingUser != null)
            {
                _logger.LogWarning("User with email {Email} already exists.", registerRequest.Email);
                throw new UserAlreadyExistsException("User already exists.");
            }

            var user = new ApplicationUser
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.Email,
                Role = Enum.Parse<UserRole>(registerRequest.Role),
                ProfilePicture = "https://api.realworld.io/images/smiley-cyrus.jpeg"
            };
            _logger.LogInformation("Creating user with email {Email}", registerRequest.Email);

            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User registration failed: {errors}");
            }
            _logger.LogInformation("User created with email {Email}", registerRequest.Email);

            await _tokenService.GenerateJwtToken(user);
            await _userManager.AddToRoleAsync(user, user.Role.ToString());
            _logger.LogInformation("User added to role {Role}", user.Role);

            return _mapper.Map<UserResponse>(user);
        }

        /// <summary>
        /// Logs in a user asynchronously.
        /// </summary>
        /// <param name="loginRequest">The login request.</param>
        /// <returns>The logged-in user response.</returns>
        /// <exception cref="ArgumentException">Thrown when the login request is invalid.</exception>
        /// <exception cref="UserNotFoundException">Thrown when the user is not found.</exception>
        public async Task<UserResponse> LoginAsync(LoginUserRequest loginRequest)
        {
            _logger.LogInformation("Attempting to log in user with email {Email}", loginRequest.Email);
            if (loginRequest == null)
            {
                _logger.LogWarning("Login request data is missing.");
                throw new ArgumentException("Login request data is missing.");
            }

            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null)
            {
                _logger.LogWarning("User with email {Email} not found.", loginRequest.Email);
                throw new UserNotFoundException("User not found.");
            }

            if (!await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                _logger.LogWarning("Invalid password for user with email {Email}.", loginRequest.Email);
                throw new ArgumentException("Invalid email or password.");
            }

            _logger.LogInformation("User with email {Email} successfully logged in.", loginRequest.Email);
            var token = await _tokenService.GenerateJwtToken(user);
            var refreshToken = await _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);
            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Token = token;
            userResponse.RefreshToken = refreshToken;

            return userResponse;
        }

        /// <summary>
        /// Gets the current user asynchronously.
        /// </summary>
        /// <returns>The current user response.</returns>
        /// <exception cref="UserNotFoundException">Thrown when the current user is not found.</exception>
        public async Task<CurrentUserResponse> GetCurrentUserAsync()
        {
            _logger.LogInformation("Getting current user.");
            var currentUserId = _currentUserService.GetUserId();
            var user = await _userManager.FindByIdAsync(currentUserId);
            if (user == null)
            {
                _logger.LogWarning("Current user not found.");
                throw new UserNotFoundException("User not found.");
            }

            _logger.LogInformation("Current user found.");
            var token = await _tokenService.GenerateJwtToken(user);
            var currentUserResponse = _mapper.Map<CurrentUserResponse>(user);
            currentUserResponse.Token = token;
            return currentUserResponse;
        }

        /// <summary>
        /// Updates the current user asynchronously.
        /// </summary>
        /// <param name="updateCurrentUserRequest">The update user request.</param>
        /// <returns>The updated current user response.</returns>
        /// <exception cref="UserNotFoundException">Thrown when the current user is not found.</exception>
        /// <exception cref="InvalidOperationException">Thrown when user update fails.</exception>
        /// <exception cref="ArgumentException">Thrown when the password is invalid.</exception>
        public async Task<CurrentUserResponse> UpdateCurrentUserAsync(UpdateUserRequest updateCurrentUserRequest)
        {
            _logger.LogInformation("Updating current user.");
            var currentUserId = _currentUserService.GetUserId();
            var user = await _userManager.FindByIdAsync(currentUserId);
            if (user == null)
            {
                _logger.LogWarning("Current user not found.");
                throw new UserNotFoundException("User not found.");
            }
            _logger.LogInformation("Current user found.");

            _mapper.Map(updateCurrentUserRequest, user);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogWarning("User update failed.");
                throw new InvalidOperationException("User update failed.");
            }

            _logger.LogInformation("User updated.");
            if (!string.IsNullOrWhiteSpace(updateCurrentUserRequest.Password))
            {
                if (!updateCurrentUserRequest.Password.Any(char.IsDigit))
                {
                    throw new ArgumentException("Password must include at least one number.");
                }
                _logger.LogInformation("Updating password for user with email {Email}", user.Email);

                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResetResult = await _userManager.ResetPasswordAsync(user, resetToken, updateCurrentUserRequest.Password);
                if (!passwordResetResult.Succeeded)
                {
                    _logger.LogWarning("Password reset failed.");
                    throw new InvalidOperationException("Password reset failed.");
                }
            }

            _logger.LogInformation("User updated.");
            var userResponse = _mapper.Map<CurrentUserResponse>(user);
            userResponse.Token = await _tokenService.GenerateJwtToken(user);
            return userResponse;
        }

        /// <summary>
        /// Refreshes the token asynchronously.
        /// </summary>
        /// <param name="request">The refresh token request.</param>
        /// <returns>The user response with new tokens.</returns>
        /// <exception cref="ArgumentException">Thrown when the refresh token is invalid.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the refresh token is invalid or expired.</exception>
        public async Task<UserResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                throw new ArgumentException("Refresh token is required.");
            }

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }

            var newAccessToken = await _tokenService.GenerateJwtToken(user);
            var newRefreshToken = await _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Token = newAccessToken;
            userResponse.RefreshToken = newRefreshToken;

            return userResponse;
        }

        /// <summary>
        /// Revokes the refresh token asynchronously.
        /// </summary>
        /// <param name="request">The refresh token request.</param>
        /// <returns>True if the refresh token was revoked successfully.</returns>
        /// <exception cref="ArgumentException">Thrown when the refresh token is invalid.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the refresh token is invalid.</exception>
        public async Task<bool> RevokeRefreshToken(RefreshTokenRequest request)
        {
            _logger.LogInformation("Revoking refresh token.");
            if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                throw new ArgumentException("Refresh token is required.");
            }
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid refresh token.");
            }

            _logger.LogInformation("Refresh token revoked.");
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.MinValue;
            await _userManager.UpdateAsync(user);

            return true;
        }

        /// <summary>
        /// Gets all users asynchronously.
        /// </summary>
        /// <returns>A list of user responses.</returns>
        /// <exception cref="UserNotFoundException">Thrown when no users are found.</exception>
        public async Task<IEnumerable<UserResponse>> GetAllUser()
        {
            _logger.LogInformation("Getting all users.");
            var users = await _userManager.Users.ToListAsync();
            if (users == null || !users.Any())
            {
                throw new UserNotFoundException("No users found.");
            }
            _logger.LogInformation("Users found.");
            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }

        /// <summary>
        /// Deletes a user asynchronously.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>The deleted user response.</returns>
        /// <exception cref="UserNotFoundException">Thrown when the user is not found.</exception>
        public async Task<UserResponse> DeleteUserAsync(int userId)
        {
            _logger.LogInformation("Deleting user with id {UserId}", userId);
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId.ToString());
            if (user == null)
            {
                throw new UserNotFoundException("User not found.");
            }
            _logger.LogInformation("User found. Deleting user with id {UserId}", userId);
            await _userManager.DeleteAsync(user);
            return _mapper.Map<UserResponse>(user);
        }

        /// <summary>
        /// Logs out the current user asynchronously.
        /// </summary>
        /// <returns>The logged-out user response.</returns>
        /// <exception cref="UserNotFoundException">Thrown when the current user is not found.</exception>
        public async Task<UserResponse> LogoutAsync()
        {
            _logger.LogInformation("Logging out user.");
            var currentUserId = _currentUserService.GetUserId();
            var user = await _userManager.FindByIdAsync(currentUserId);
            if (user == null)
            {
                throw new UserNotFoundException("User not found.");
            }
            _logger.LogInformation("User logged out.");
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.MinValue;
            await _userManager.UpdateAsync(user);
            return _mapper.Map<UserResponse>(user);
        }
    }
}
