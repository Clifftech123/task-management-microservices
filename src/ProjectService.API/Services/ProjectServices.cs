using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectService.API.Domain.Entities;
using ProjectService.API.Infrastructure.Context;
using ProjectService.API.Infrastructure.Exceptions;
using static ProjectService.API.Domain.Contracts.ProjectDtos;

namespace ProjectService.API.Services
{
    public class ProjectServices : IProjectServices
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectServices> _logger;
        private readonly ProjectDbContext _projectDbContext;

        public ProjectServices(IMapper mapper, ILogger<ProjectServices> logger, ProjectDbContext projectDbContext)
        {
            _mapper = mapper;
            _logger = logger;
            _projectDbContext = projectDbContext;
        }

        public async Task<Project> CreateProjectAsync(CreateProjectRequest createProjectRequest)
        {
            var project = _mapper.Map<Project>(createProjectRequest);
            await _projectDbContext.Projects.AddAsync(project);
            await _projectDbContext.SaveChangesAsync();
            return project;
        }

        public async Task<ProjectResponse> GetProjectByIdAsync(int id)
        {
            var project = await _projectDbContext.Projects.FindAsync(id);
            if (project == null)
            {
                _logger.LogError($"Project with id: {id} not found");
                throw new ProjectNotFoundExceptions(projectId: id);
            }
            return _mapper.Map<ProjectResponse>(project);
        }

        public async Task<IEnumerable<ProjectResponse>> GetProjectsAsync()
        {
            var projects = await _projectDbContext.Projects.ToListAsync();
            if (!projects.Any())
            {
                _logger.LogError("No projects found");
                throw new KeyNotFoundException("No projects found");
            }
            return _mapper.Map<IEnumerable<ProjectResponse>>(projects);
        }

        public async Task<UpdateProjectRequest> UpdateProjectAsync(UpdateProjectRequest updateProjectRequest)
        {
            var project = await _projectDbContext.Projects.FindAsync(updateProjectRequest.Name);
            if (project == null)
            {
                _logger.LogError($"Project with id: {updateProjectRequest.Name} not found");
                throw new KeyNotFoundException($"Project with id: {updateProjectRequest.Name} not found");
            }
            _mapper.Map(updateProjectRequest, project);
            await _projectDbContext.SaveChangesAsync();
            return updateProjectRequest;
        }

        public async Task<ProjectResponse> DeleteProjectAsync(int id)
        {
            var project = await _projectDbContext.Projects.FindAsync(id);
            if (project == null)
            {
                _logger.LogError($"Project with id: {id} not found");
                throw new KeyNotFoundException($"Project with id: {id} not found");
            }
            _projectDbContext.Projects.Remove(project);
            await _projectDbContext.SaveChangesAsync();
            return _mapper.Map<ProjectResponse>(project);
        }
    }
}
