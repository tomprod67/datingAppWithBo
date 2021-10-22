using System.Linq;
using DTO.DTOs;
using Service.BOs;
using Service.DTOs;
using DB.Entities;
using Generic.Extensions;
using AutoMapper;

namespace Service.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap(typeof(PagedList<>), typeof(PagedList<>));


            CreateMap<AppUser, MemberBo>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<MemberBo, MemberDto>()
                .ForMember(dest => dest.KnownAs, opt => opt.MapFrom(src => src.KnownAs));
            CreateMap<PagedList<MemberBo>, PagedList<MemberDto>>();
            CreateMap<PagedList<MemberDto>, PagedList<MemberBo>>();     // TODO tester si l'on peut l'enlever 

            // Faire un mapping = Premier element dest est le champ qu'on veut ajouter et qui n'est pas présent dans la src / Et la src est ou se trouve l'élément qu'on va mettre en place pour compléter la dest

            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src => src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));

            CreateMap<UserBo, UserDto>();

            CreateMap<LikeBo, LikeDto>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, act => act.MapFrom(src => src.Username))
                .ForMember(dest => dest.Age, act => act.MapFrom(src => src.Age))
                .ForMember(dest => dest.KnownAs, act => act.MapFrom(src => src.KnownAs))
                .ForMember(dest => dest.PhotoUrl, act => act.MapFrom(src => src.PhotoUrl))
                .ForMember(dest => dest.City, act => act.MapFrom(src => src.City));
            CreateMap<PagedList<UserBo>, PagedList<LikeDto>>();
            CreateMap<PagedList<UserDto>, PagedList<LikeBo>>(); // TODO tester si l'on peut l'enlever 
        }
    }
}