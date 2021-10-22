using AutoMapper;
using DB.Data;
using Service.BOs;
using Service.Interfaces;
using System.Threading.Tasks;

namespace Service.Repositories
{
    public class AccountRepository : IAccountRepository
    {

        public AccountRepository()
        {
        }

        public async Task<UserBo> RegisterUser(string username, Task<string> token, string knowAs, string gender)
        {
            return new UserBo
            {
                Username = username,
                Token = await token,
                KnownAs = knowAs,
                Gender = gender,
            };
        }

        public async Task<UserBo> LoginUser(string username, Task<string> token, string photoUrl, string knowAs, string gender)
        {
            return new UserBo
            {
                Username = username,
                Token = await token,
                PhotoUrl = photoUrl,
                KnownAs = knowAs,
                Gender = gender,
            };
        }

    }
}
