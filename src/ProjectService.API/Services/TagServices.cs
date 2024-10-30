using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectService.API.Domain.Contracts;
using ProjectService.API.Domain.Entities;
using ProjectService.API.Infrastructure.Context;

namespace ProjectService.API.Services
{
    public class TagServices : ITagsServices
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TagServices> _logger;
        private readonly ProjectDbContext _projectDbContext;

        public TagServices(IMapper mapper, ILogger<TagServices> logger, ProjectDbContext projectDbContext)
        {
            _mapper = mapper;
            _logger = logger;
            _projectDbContext = projectDbContext;
        }

        public async Task<TagesDtos.CreateTagRequest> CreateTagAsync(TagesDtos.CreateTagRequest createTagRequest)
        {
            var creatTag = _mapper.Map<Tags>(createTagRequest);
            await _projectDbContext.Tags.AddAsync(creatTag);
            await _projectDbContext.SaveChangesAsync();
            return createTagRequest;
        }

        public async Task<TagesDtos.TagResponse> GetTagByIdAsync(int id)
        {
            var tag = await _projectDbContext.Tags.FindAsync(id);
            if (tag == null)
            {
                _logger.LogError($"Tag with id: {id} not found");
                throw new KeyNotFoundException($"Tag with id: {id} not found");
            }
            return _mapper.Map<TagesDtos.TagResponse>(tag);
        }

        public async Task<IEnumerable<TagesDtos.TagResponse>> GetTagsAsync()
        {
            var findTags = await _projectDbContext.Tags.ToListAsync();
            if (!findTags.Any())
            {
                _logger.LogError("No tags found");
                throw new KeyNotFoundException("No tags found");
            }
            return _mapper.Map<IEnumerable<TagesDtos.TagResponse>>(findTags);
        }

        public async Task<TagesDtos.UpdateTagRequest> UpdateTagAsync(TagesDtos.UpdateTagRequest updateTagRequest)
        {
            var existingTag = await _projectDbContext.Tags.FindAsync(updateTagRequest.Name);
            if (existingTag == null)
            {
                _logger.LogError($"Tag with id: {updateTagRequest.Name} not found");
                throw new KeyNotFoundException($"Tag with id: {updateTagRequest.Name} not found");
            }
            _mapper.Map(updateTagRequest, existingTag);
            await _projectDbContext.SaveChangesAsync();
            return updateTagRequest;
        }

        public async Task<TagesDtos.TagResponse> DeleteTagAsync(int id)
        {
            var tag = await _projectDbContext.Tags.FindAsync(id);
            if (tag == null)
            {
                _logger.LogError($"Tag with id: {id} not found");
                throw new KeyNotFoundException($"Tag with id: {id} not found");
            }
            _projectDbContext.Tags.Remove(tag);
            await _projectDbContext.SaveChangesAsync();
            return _mapper.Map<TagesDtos.TagResponse>(tag);
        }
    }
}
