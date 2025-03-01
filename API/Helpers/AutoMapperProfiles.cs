using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;
using System.Linq;

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
            CreateMap<RegisterDto, AppUser>();
            CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s));
        }
    }

    public class MainPhotoUrlResolver : IValueResolver<AppUser, MemberDto, string?>
    {
        public string? Resolve(AppUser source, MemberDto destination, string? destMember, ResolutionContext context)
        {
            if (source.Photos == null || !source.Photos.Any())
            {
                return "https://dummyimage.com/200x200/0000FF/808080&text=No+Profile+Picture";
            }
            
            var mainPhoto = source.Photos.FirstOrDefault(x => x.IsMain);
            return mainPhoto?.Url ?? "https://dummyimage.com/200x200/0000FF/808080&text=No+Profile+Picture";
        }
    }
}
