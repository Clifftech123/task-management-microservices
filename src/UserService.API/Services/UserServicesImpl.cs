using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UserService.API.Domain.Contracts;
using UserService.API.Domain.Entities;
using UserService.API.Infrastructure.Exceptions;

namespace UserService.API.Services
{
    public class UserServiceImple : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserServiceImple> _logger;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserServiceImple"/> class.
        /// </summary>
        /// <param name="tokenService">The token service.</param>
        /// <param name="userManager">The user manager.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="currentUserService">The current user service.</param>
        /// <param name="logger">The logger.</param>
        public UserServiceImple(ITokenService tokenService, UserManager<ApplicationUser> userManager, IMapper mapper, ICurrentUserService currentUserService, ILogger<UserServiceImple> logger)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<UserResponse> RegisterAsync(UserRegisterRequest registerRequest)
        {
            _logger.LogInformation("Registering user with email {Email}", registerRequest.Email);
            if (registerRequest == null)
            {
                throw new ArgumentException("User registration data is missing.");
            }

            // Validate the password
            if (!registerRequest.Password.Any(char.IsDigit))
            {
                _logger.LogWarning("Password does not contain a number.");
                throw new ArgumentException("Password must include at least one number.");
            }

            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("User with email {Email} already exists.", registerRequest.Email);
                throw new UserAlreadyExistsException("User already exists.");
            }

            // Set up default profile picture
            var defaultProfilePicture = "https://api.realworld.io/images/smiley-cyrus.jpeg";

            var user = new ApplicationUser
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.Email,
                Role = registerRequest.Role,
                ProfilePicture = defaultProfilePicture
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

            // Return user information
            return _mapper.Map<ApplicationUser, UserResponse>(user);
        }

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
            var userResponse = _mapper.Map<ApplicationUser, UserResponse>(user);
            userResponse.Token = token;

            return userResponse;
        }

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
            var currentUserResponse = _mapper.Map<ApplicationUser, CurrentUserResponse>(user);
            currentUserResponse.Token = token;
            return currentUserResponse;
        }

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
            var userResponse = _mapper.Map<ApplicationUser, CurrentUserResponse>(user);
            userResponse.Token = await _tokenService.GenerateJwtToken(user);
            return userResponse;
        }
    }
}
