namespace UserService.API.Domain.Contracts
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public string ProfilePicture { get; set; }
    }

    public class UserRegisterRequest
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class UpdateUserRequest
    {

        public string Email { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string ProfilePicture { get; set; }
    }

    public class LoginUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class CurrentUserResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string ProfilePicture { get; set; }
    }
}
