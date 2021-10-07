using DB.Entities;

namespace Service.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}