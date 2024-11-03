using Microsoft.AspNetCore.Mvc;
using ProjectService.API.Services;
using static ProjectService.API.Domain.Contracts.TagesDtos;

namespace ProjectService.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class TagsController : ControllerBase
    {
        private readonly ITagsServices _tagService;

        public TagsController(ITagsServices tagService)
        {
            _tagService = tagService;
        }

        [HttpGet("tags")]
        public async Task<IActionResult> GetTagsAsync()
        {
            try
            {
                var response = await _tagService.GetTagsAsync();
                if (response == null || !response.Any())
                {
                    return NotFound(new { message = "No tags found" });
                }
                return Ok(new { message = "Tags   successfully feted  ", response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching tags", error = ex.Message });
            }
        }

        [HttpGet("tags/{id}")]
        public async Task<IActionResult> GetTagByIdAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = await _tagService.GetTagByIdAsync(id);
                if (response == null)
                {
                    return NotFound(new { message = "Tag not found" });
                }
                return Ok(new { message = "Tags   successfully feted  ", response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the tag", error = ex.Message });
            }
        }

        [HttpPost("tags")]
        public async Task<IActionResult> CreateTagAsync(CreateTagRequest createTagRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = await _tagService.CreateTagAsync(createTagRequest);
                return Ok(new { message = "Tag created successfully", response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the tag", error = ex.Message });
            }
        }

        [HttpPut("tags/{id}")]
        public async Task<IActionResult> UpdateTagAsync(int id, UpdateTagRequest updateTagRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var existingTag = await _tagService.GetTagByIdAsync(id);
                if (existingTag == null)
                {
                    return NotFound(new { message = "Tag not found" });
                }
                await _tagService.UpdateTagAsync(updateTagRequest);
                return Ok(new { message = "Tag updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the tag", error = ex.Message });
            }
        }

        [HttpDelete("tags/{id}")]
        public async Task<IActionResult> DeleteTagAsync(int id)
        {
            try
            {
                var existingTag = await _tagService.GetTagByIdAsync(id);
                if (existingTag == null)
                {
                    return NotFound(new { message = "Tag not found" });
                }
                await _tagService.DeleteTagAsync(id);
                return Ok(new { message = "Tag deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the tag", error = ex.Message });
            }
        }
    }
}
