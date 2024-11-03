using Microsoft.AspNetCore.Mvc;
using ProjectService.API.Services;
using static ProjectService.API.Domain.Contracts.ProjectDtos;

namespace ProjectService.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectServices _projectServices;

        public ProjectController(IProjectServices projectServices)
        {
            _projectServices = projectServices;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetProjectsAsync()
        {
            try
            {
                var projects = await _projectServices.GetProjectsAsync();
                if (projects == null || !projects.Any())
                {
                    return NotFound(new { message = "No projects found" });
                }
                return Ok(new { message = "Projects successfully fetched", projects });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching projects", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectByIdAsync(int id)
        {
            try
            {
                var project = await _projectServices.GetProjectByIdAsync(id);
                if (project == null)
                {
                    return NotFound(new { message = "Project not found" });
                }
                return Ok(new { message = "Project successfully fetched", project });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the project", error = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProjectAsync(CreateProjectRequest createProjectRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var project = await _projectServices.CreateProjectAsync(createProjectRequest);
                return Ok(new { message = "Project created successfully", project });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the project", error = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProjectAsync(int id, UpdateProjectRequest updateProjectRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var project = await _projectServices.UpdateProjectAsync(id, updateProjectRequest);
                return Ok(new { message = "Project updated successfully", project });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the project", error = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProjectAsync(int id)
        {
            try
            {
                var project = await _projectServices.DeleteProjectAsync(id);
                return Ok(new { message = "Project deleted successfully", project });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the project", error = ex.Message });
            }
        }
    }
}
