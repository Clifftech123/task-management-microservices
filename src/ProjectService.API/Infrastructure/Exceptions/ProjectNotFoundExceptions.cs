namespace ProjectService.API.Infrastructure.Exceptions
{
    public class ProjectNotFoundExceptions : Exception
    {
        public ProjectNotFoundExceptions(int projectId) : base($"Project with id {projectId} was not found.")
        {
        }
    }
}
