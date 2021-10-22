using DB.Entities;
using Service.BOs;
using Service.Helpers;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likeUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PagedList<LikeBo>> GetUserLikes(LikesParams likesParams);
    }
}
