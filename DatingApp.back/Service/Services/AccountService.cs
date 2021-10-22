using AutoMapper;
using DTO.DTOs;
using Service.BOs;
using Service.Interfaces;
using System.Threading.Tasks;

namespace Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public AccountService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public UserDto RegisterUser(string username, Task<string> token, string knowAs, string gender)
        {
            Task<UserBo> user = unitOfWork.AccountRepository.RegisterUser(username, token, knowAs, gender);
            return mapper.Map<UserBo, UserDto>(user.Result);
        }


        public UserDto LoginUser(string username, Task<string> token, string photoUrl, string knowAs, string gender)
        {
            Task<UserBo> user = unitOfWork.AccountRepository.LoginUser(username, token, photoUrl, knowAs, gender);
            return mapper.Map<UserBo, UserDto>(user.Result);

        }

    }
}
