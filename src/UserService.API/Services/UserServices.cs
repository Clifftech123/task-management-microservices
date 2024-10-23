using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UserService.API.Domain.Contracts;
using UserService.API.Domain.Entities;

namespace UserService.API.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UserService(ITokenService tokenService, UserManager<ApplicationUser> userManager, IMapper mapper,ICurrentUserService currentUserService)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<UserResponse> RegisterAsync(UserRegisterRequest registerRequest)
        {
            if (registerRequest == null)
            {
                throw new ArgumentException("User registration data is missing.");
            }

            // Validate the password
            if (!registerRequest.Password.Any(char.IsDigit))
            {
                throw new ArgumentException("Password must include at least one number.");
            }

            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("User already exists with this email.");
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

            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User registration failed: {errors}");
            }

            await _tokenService.GenerateJwtToken(user);

            // Return user information
            return _mapper.Map<ApplicationUser, UserResponse>(user);
        }

        public async Task<UserResponse> LoginAsync(LoginUserRequest loginRequest)
        {
            if (loginRequest == null)
            {
                throw new ArgumentException("Login request data is missing.");
            }

            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                throw new ArgumentException("Invalid email or password.");
            }

            var token = await _tokenService.GenerateJwtToken(user);
            var userResponse = _mapper.Map<ApplicationUser, UserResponse>(user);
            userResponse.Token = token;

            return userResponse;
        }

        public async Task<CurrentUserResponse> GetCurrentUserAsync()
        {
            var currentUserId = _currentUserService.GetUserId();
            var user = await _userManager.FindByIdAsync(currentUserId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var token = await _tokenService.GenerateJwtToken(user);
            var currentUserResponse = _mapper.Map<ApplicationUser, CurrentUserResponse>(user);
            currentUserResponse.Token = token;
            return currentUserResponse;
        }

        public async Task<CurrentUserResponse> UpdateCurrentUserAsync(UpdateUserRequest updateRequest)
        {
            var currentUserId = _currentUserService.GetUserId();
            var user = await _userManager.FindByIdAsync(currentUserId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            _mapper.Map(updateRequest, user);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("User update failed.");
            }

            if (!string.IsNullOrWhiteSpace(updateRequest.Password))
            {
                if (!updateRequest.Password.Any(char.IsDigit))
                {
                    throw new ArgumentException("Password must include at least one number.");
                }

                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResetResult = await _userManager.ResetPasswordAsync(user, resetToken, updateRequest.Password);
                if (!passwordResetResult.Succeeded)
                {
                    throw new InvalidOperationException("Password reset failed.");
                }
            }

            var userResponse = _mapper.Map<ApplicationUser, CurrentUserResponse>(user);
            userResponse.Token = await _tokenService.GenerateJwtToken(user);
            return userResponse;
        }
    }
}
