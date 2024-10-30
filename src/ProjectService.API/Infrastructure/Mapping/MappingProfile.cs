using AutoMapper;
using ProjectService.API.Domain.Entities;
using static ProjectService.API.Domain.Contracts.ProjectDtos;
using static ProjectService.API.Domain.Contracts.TagesDtos;

namespace ProjectService.API.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(x => x.Name).ToList()));
            CreateMap<CreateProjectRequest, Project>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
              .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
              .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
              .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(x => new Tags { Name = x }).ToList()));


            CreateMap<UpdateProjectRequest, Project>()

                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(x => new Tags { Name = x }).ToList()));

            CreateMap<Tags, TagResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<CreateTagRequest, Tags>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<UpdateTagRequest, Tags>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
