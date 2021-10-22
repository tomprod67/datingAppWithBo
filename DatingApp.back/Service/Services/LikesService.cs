using AutoMapper;
using DTO.DTOs;
using Service.BOs;
using Service.Helpers;
using Service.Interfaces;

namespace Service.Services
{
    public class LikesService : ILikesService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public LikesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public PagedList<LikeDto> GetUserLikes(LikesParams likesParams)
        {
            PagedList<LikeBo> listUserLike = unitOfWork.LikesRepository.GetUserLikes(likesParams).Result;
            PagedList<LikeDto> listMapping = mapper.Map<PagedList<LikeBo>, PagedList<LikeDto>>(listUserLike);

            for (int i = 0; i < listUserLike.Count; i++)
            {
                LikeDto elementMapping = mapper.Map<LikeBo, LikeDto>(listUserLike[i]);
                listMapping.Add(elementMapping);
            }
            return listMapping;
        }
    }
}
