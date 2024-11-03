using UserService.API.Domain.Contracts;

namespace UserService.API.Services
{
    public interface IUserService
    {
        /// <summary>
        ///  Method to register a new user
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        Task<UserResponse> RegisterAsync(UserRegisterRequest registerRequest);

        /// <summary>
        ///  Method to login a user
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        Task<UserResponse> LoginAsync(LoginUserRequest loginRequest);


        /// <summary>
        ///  Delete a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserResponse> DeleteUserAsync(int userId);

        /// <summary>
        ///  Method to get all Users
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserResponse>> GetAllUser();

        /// <summary>
        ///  Get current user
        /// </summary>
        /// <returns></returns>
        Task<CurrentUserResponse> GetCurrentUserAsync();




        /// <summary>
        ///  Logout the user
        /// </summary>
        /// <returns></returns>
        Task<UserResponse> LogoutAsync();


        /// <summary>
        ///  Revoke the refresh token
        /// </summary>
        /// <param name="refreshTokenRemoveRequest"></param>
        /// <returns></returns>
        Task<bool> RevokeRefreshToken(RefreshTokenRequest refreshTokenRemoveRequest);

        /// <summary>
        ///  Refresh the access token using a refresh token
        /// </summary>
        /// <param name="refreshTokenRequest"></param>
        /// <returns></returns>
        Task<UserResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);



    }
}
