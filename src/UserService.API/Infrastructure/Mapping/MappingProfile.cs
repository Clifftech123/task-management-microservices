using AutoMapper;
using UserService.API.Domain.Contracts;
using UserService.API.Domain.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationUser, UserResponse>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture))
            .ReverseMap();


        CreateMap<ApplicationUser, CurrentUserResponse>()

         .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
         .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
         .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
         .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture))
         .ReverseMap();

    }
}
