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
            .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture))
             .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.RefreshToken))
             .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken))
             .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
             .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
             .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))

            .ReverseMap();

        CreateMap<ApplicationUser, CurrentUserResponse>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))

            .ReverseMap();

        CreateMap<ApplicationUser, GetAllUserReponse>();



    }
}