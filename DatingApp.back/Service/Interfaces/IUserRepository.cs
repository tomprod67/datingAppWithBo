using System.Collections.Generic;
using System.Threading.Tasks;
using Service.BOs;
using DB.Entities;
using Service.Helpers;

namespace Service.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<PagedList<MemberBo>> GetMembersAsync(UserParams userParams);
        Task<MemberBo> GetMemberAsync(string username);
        Task<string> GetUserGender(string username);
    }
}