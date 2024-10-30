using Microsoft.AspNetCore.Mvc;
using ProjectService.API.Domain.Entities;
using ProjectService.API.Services;
using static ProjectService.API.Domain.Contracts.ProjectDtos;

namespace ProjectService.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectServices _projectServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectController"/> class.
        /// </summary>
        /// <param name="projectServices">The project services.</param>
        public ProjectController(IProjectServices projectServices)
        {
            _projectServices = projectServices;
        }

        /// <summary>
        /// Gets the list of projects.
        /// </summary>
        /// <returns>A list of <see cref="ProjectResponse"/>.</returns>
        [HttpGet("all")]
        public async Task<IEnumerable<ProjectResponse>> GetProjectsAsync()
        {
            return await _projectServices.GetProjectsAsync();
        }

        /// <summary>
        /// Gets the project by identifier.
        /// </summary>
        /// <param name="id">The project identifier.</param>
        /// <returns>A <see cref="ProjectResponse"/>.</returns>
        [HttpGet("{id}")]
        public async Task<ProjectResponse> GetProjectByIdAsync(int id)
        {
            return await _projectServices.GetProjectByIdAsync(id);
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="createProjectRequest">The create project request.</param>
        /// <returns>The created <see cref="Project"/>.</returns>
        [HttpPost("create")]
        public async Task<Project> CreateProjectAsync(CreateProjectRequest createProjectRequest)
        {
            return await _projectServices.CreateProjectAsync(createProjectRequest);
        }

        /// <summary>
        /// Updates an existing project.
        /// </summary>
        /// <param name="updateProjectRequest">The update project request.</param>
        /// <returns>The updated <see cref="UpdateProjectRequest"/>.</returns>
        [HttpPut("update")]
        public async Task<UpdateProjectRequest> UpdateProjectAsync(UpdateProjectRequest updateProjectRequest)
        {
            return await _projectServices.UpdateProjectAsync(updateProjectRequest);
        }

        /// <summary>
        /// Deletes the project by identifier.
        /// </summary>
        /// <param name="id">The project identifier.</param>
        /// <returns>A <see cref="ProjectResponse"/>.</returns>
        [HttpDelete("delete/{id}")]
        public async Task<ProjectResponse> DeleteProjectAsync(int id)
        {
            return await _projectServices.DeleteProjectAsync(id);
        }
    }
}
