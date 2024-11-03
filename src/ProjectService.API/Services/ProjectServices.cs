using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectService.API.Domain.Entities;
using ProjectService.API.Infrastructure.Context;
using static ProjectService.API.Domain.Contracts.ProjectDtos;

namespace ProjectService.API.Services
{
    /// <summary>
    /// Service for managing projects.
    /// </summary>
    public class ProjectServices : IProjectServices
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectServices> _logger;
        private readonly ProjectDbContext _projectDbContext;
        private readonly ICurrentUserService currentUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectServices"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="projectDbContext">The project database context.</param>
        public ProjectServices(IMapper mapper, ILogger<ProjectServices> logger, ProjectDbContext projectDbContext, ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _logger = logger;
            _projectDbContext = projectDbContext;
            this.currentUserService = currentUserService;

        }

        /// <summary>
        /// Creates a new project asynchronously.
        /// </summary>
        /// <param name="createProjectRequest">The create project request.</param>
        /// <returns>The created project.</returns>
        /// <returns>The created project.</returns>
        public async Task<Project> CreateProjectAsync(CreateProjectRequest createProjectRequest)
        {
            try
            {
                _logger.LogInformation("Creating project for user {UserId}", currentUserService.GetUserId());
                var project = _mapper.Map<Project>(createProjectRequest);
                project.UserId = currentUserService.GetUserId();
                await _projectDbContext.Projects.AddAsync(project);

                await _projectDbContext.SaveChangesAsync();
                return project;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating project for user {UserId}", currentUserService.GetUserId());
                throw new ApplicationException("An error occurred while creating the project.", ex);
            }
        }


        /// <summary>
        /// Gets a project by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The project identifier.</param>
        /// <returns>The project response.</returns>
        public async Task<ProjectResponse> GetProjectByIdAsync(int id)
        {
            var project = await _projectDbContext.Projects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (project == null)
            {
                _logger.LogError("Project with id: {ProjectId} not found", id);
                throw new KeyNotFoundException($"Project with id: {id} not found");
            }
            return _mapper.Map<ProjectResponse>(project);
        }

        /// <summary>
        /// Gets all projects asynchronously.
        /// </summary>
        /// <returns>A list of project responses.</returns>
        public async Task<IEnumerable<ProjectResponse>> GetProjectsAsync()
        {
            var projects = await _projectDbContext.Projects.AsNoTracking().ToListAsync();
            if (projects.Count == 0)
            {
                _logger.LogError("No projects found");
                throw new KeyNotFoundException("No projects found");
            }
            return _mapper.Map<IEnumerable<ProjectResponse>>(projects);
        }

        /// <summary>
        /// Updates a project asynchronously.
        /// </summary>
        /// <param name="id">The project identifier.</param>
        /// <param name="updateProjectRequest">The update project request.</param>
        /// <returns>The updated project request.</returns>
        public async Task<Project> UpdateProjectAsync(int id, UpdateProjectRequest updateProjectRequest)
        {
            var project = await _projectDbContext.Projects.FindAsync(id);
            if (project == null)
            {
                _logger.LogError("Project with id: {ProjectId} not found", id);
                throw new KeyNotFoundException($"Project with id: {id} not found");
            }

            if (!string.IsNullOrEmpty(updateProjectRequest.Name))
            {
                project.Name = updateProjectRequest.Name;
            }
            if (!string.IsNullOrEmpty(updateProjectRequest.Description))
            {
                project.Description = updateProjectRequest.Description;
            }
            if (updateProjectRequest.StartDate != default)
            {
                project.StartDate = updateProjectRequest.StartDate;
            }
            if (updateProjectRequest.EndDate != default)
            {
                project.EndDate = updateProjectRequest.EndDate;
            }
            if (updateProjectRequest.Tags != null)
            {
                project.Tags = updateProjectRequest.Tags.Select(tag => new Tags { Name = tag }).ToList();
            }

            await _projectDbContext.SaveChangesAsync();
            return project;
        }

        /// <summary>
        /// Deletes a project asynchronously.
        /// </summary>
        /// <param name="id">The project identifier.</param>
        /// <returns>The deleted project response.</returns>
        public async Task<ProjectResponse> DeleteProjectAsync(int id)
        {
            var project = await _projectDbContext.Projects.FindAsync(id);
            if (project == null)
            {
                _logger.LogError("Project with id: {ProjectId} not found", id);
                throw new KeyNotFoundException($"Project with id: {id} not found");
            }

            _projectDbContext.Projects.Remove(project);
            await _projectDbContext.SaveChangesAsync();
            return _mapper.Map<ProjectResponse>(project);
        }
    }
}
