using DTO.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IAccountService
    {
        UserDto RegisterUser(string username, Task<string> token, string knowAs, string gender);
        UserDto LoginUser(string username, Task<string> token, string photoUrl, string knowAs, string gender);
    }
}
