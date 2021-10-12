using DB.Entities;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}