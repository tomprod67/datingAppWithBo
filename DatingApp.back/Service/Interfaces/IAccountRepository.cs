using Microsoft.AspNetCore.Mvc;
using Service.BOs;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IAccountRepository
    {
        Task<UserBo> RegisterUser(string username, Task<string> token, string knowAs, string gender);
        Task<UserBo> LoginUser(string username, Task<string> token, string photoUrl, string knowAs, string gender);
    }
}
