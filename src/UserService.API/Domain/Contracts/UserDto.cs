namespace UserService.API.Domain.Contracts
{
    /// <summary>
    /// Represents the response for a user.
    /// </summary>
    public class UserResponse
    {


        public string FirstName { get; set; }
        /// <summary>
        ///  Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        ///   Gets or sets user Gender
        /// </summary>
        public string Gender { get; set; }


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

        public string RefreshToken { get; set; }
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        public string? AccessToken { get; set; }

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
        ///  User first name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        ///  Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        ///   Gets or sets user Gender
        /// </summary>
        public string Gender { get; set; }

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

    public class GetAllUserReponse
    {

        public string FirstName { get; set; }
        /// <summary>
        ///  Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        ///   Gets or sets user Gender
        /// </summary>
        public string Gender { get; set; }


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

    }

}


/// <summary>
/// Represents the response for the current user.
/// </summary>
public class CurrentUserResponse
{
    public string FirstName { get; set; }
    /// <summary>
    ///  Gets or sets the last name.
    /// </summary>
    public string LastName { get; set; }
    /// <summary>
    ///   Gets or sets user Gender
    /// </summary>
    public string Gender { get; set; }


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

    public string? ProfilePicture { get; set; }



}







