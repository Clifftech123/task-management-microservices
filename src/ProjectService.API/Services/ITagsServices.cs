using static ProjectService.API.Domain.Contracts.TagesDtos;

namespace ProjectService.API.Services
{
    public interface ITagsServices
    {
        Task<IEnumerable<TagResponse>> GetTagsAsync();
        Task<TagResponse> GetTagByIdAsync(int id);
        Task<CreateTagRequest> CreateTagAsync(CreateTagRequest createTagRequest);
        Task<UpdateTagRequest> UpdateTagAsync(UpdateTagRequest updateTagRequest);

        Task<TagResponse> DeleteTagAsync(int id);
    }
}
