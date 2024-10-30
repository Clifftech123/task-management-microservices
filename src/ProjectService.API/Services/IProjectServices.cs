using ProjectService.API.Domain.Entities;
using static ProjectService.API.Domain.Contracts.ProjectDtos;

namespace ProjectService.API.Services
{
    public interface IProjectServices
    {
        Task<IEnumerable<ProjectResponse>> GetProjectsAsync();
        Task<ProjectResponse> GetProjectByIdAsync(int id);
        Task<Project> CreateProjectAsync(CreateProjectRequest createProjectRequest);
        Task<UpdateProjectRequest> UpdateProjectAsync(UpdateProjectRequest updateProjectRequest);
        Task<ProjectResponse> DeleteProjectAsync(int id);
    }
}
