using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl,
                    opt => opt.MapFrom<MainPhotoUrlResolver>())
                .ForMember(dest => dest.Age,
                    opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
        }
    }

    public class MainPhotoUrlResolver : IValueResolver<AppUser, MemberDto, string>
    {
        public string Resolve(AppUser source, MemberDto destination, string destMember, ResolutionContext context)
        {
            var mainPhoto = source.Photos.FirstOrDefault(x => x.IsMain);
            return mainPhoto?.Url ?? "default-url";
        }
    }
}
