using Microsoft.AspNetCore.Mvc;
using ProjectService.API.Services;
using static ProjectService.API.Domain.Contracts.TagesDtos;

namespace ProjectService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagsServices _tagService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagsController"/> class.
        /// </summary>
        /// <param name="tagService">The tag services.</param>
        public TagsController(ITagsServices tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// Gets the list of tags.
        /// </summary>
        /// <returns>A list of <see cref="TagResponse"/>.</returns>
        [HttpGet]
        public async Task<IEnumerable<TagResponse>> GetTagsAsync()
        {
            return await _tagService.GetTagsAsync();
        }

        /// <summary>
        /// Gets the tag by identifier.
        /// </summary>
        /// <param name="id">The tag identifier.</param>
        /// <returns>A <see cref="TagResponse"/>.</returns>
        [HttpGet("{id}")]
        public async Task<TagResponse> GetTagByIdAsync(int id)
        {
            return await _tagService.GetTagByIdAsync(id);
        }

        /// <summary>
        /// Creates a new tag.
        /// </summary>
        /// <param name="createTagRequest">The create tag request.</param>
        /// <returns>The created <see cref="CreateTagRequest"/>.</returns>
        [HttpPost]
        public async Task<CreateTagRequest> CreateTagAsync(CreateTagRequest createTagRequest)
        {
            return await _tagService.CreateTagAsync(createTagRequest);
        }

        /// <summary>
        /// Updates an existing tag.
        /// </summary>
        /// <param name="updateTagRequest">The update tag request.</param>
        /// <returns>The updated <see cref="UpdateTagRequest"/>.</returns>
        [HttpPut]
        public async Task<UpdateTagRequest> UpdateTagAsync(UpdateTagRequest updateTagRequest)
        {
            return await _tagService.UpdateTagAsync(updateTagRequest);
        }

        /// <summary>
        /// Deletes the tag by identifier.
        /// </summary>
        /// <param name="id">The tag identifier.</param>
        /// <returns>A <see cref="TagResponse"/>.</returns>
        [HttpDelete("{id}")]
        public async Task<TagResponse> DeleteTagAsync(int id)
        {
            return await _tagService.DeleteTagAsync(id);
        }
    }
}
