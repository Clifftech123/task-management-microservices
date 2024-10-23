using UserService.API.Domain.Contracts;

namespace UserService.API.Services
{
    public interface IUserService
    {
     
        Task<UserResponse> RegisterAsync(UserRegisterRequest registerRequest);
        Task<UserResponse> LoginAsync(LoginUserRequest loginRequest);
        Task<CurrentUserResponse> GetCurrentUserAsync();
        Task<CurrentUserResponse> UpdateCurrentUserAsync(UpdateUserRequest updateCurrentUserRequest);
    }
}
