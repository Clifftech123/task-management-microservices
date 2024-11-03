namespace ProjectService.API.Services
{
    /// <summary>
    ///  it will return the current user id
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        ///  it will return the current user id
        /// </summary>
        /// <returns></returns>
        string GetUserId();
    }
}
