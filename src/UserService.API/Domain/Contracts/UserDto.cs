namespace UserService.API.Domain.Contracts
{
    /// <summary>
    /// Represents the response for a user.
    /// </summary>
    public class UserResponse
    {
        /// <summary>


        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public string? Role { get; set; }


        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Gets or sets the profile picture.
        /// </summary>
        public string? ProfilePicture { get; set; }
    }

    /// <summary>
    /// Represents the request to register a user.
    /// </summary>
    public class UserRegisterRequest
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public string? Role { get; set; }
    }

    /// <summary>
    /// Represents the request to update a user.
    /// </summary>
    public class UpdateUserRequest
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public string? Role { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the profile picture.
        /// </summary>
        public string? ProfilePicture { get; set; }
    }

    /// <summary>
    /// Represents the request to log in a user.
    /// </summary>
    public class LoginUserRequest
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string? Password { get; set; }
    }

    /// <summary>
    /// Represents the response for the current user.
    /// </summary>
    public class CurrentUserResponse
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public string? Role { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Gets or sets the profile picture.
        /// </summary>
        public string? ProfilePicture { get; set; }
    }
}
