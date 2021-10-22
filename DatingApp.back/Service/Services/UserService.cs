using AutoMapper;
using DTO.DTOs;
using Service.BOs;
using Service.Helpers;
using Service.Interfaces;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            MemberBo memberBo = await unitOfWork.UserRepository.GetMemberAsync(username);
            return mapper.Map<MemberBo, MemberDto>(memberBo);
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            PagedList<MemberBo> users = await unitOfWork.UserRepository.GetMembersAsync(userParams);

            PagedList<MemberDto> listMapping = mapper.Map<PagedList<MemberBo>, PagedList<MemberDto>>(users);

            for (int i = 0; i < users.Count; i++)
            {
                MemberDto elementMapping = mapper.Map<MemberBo, MemberDto>(users[i]);
                listMapping.Add(elementMapping);
            }
            return listMapping;
        }
    }
}
