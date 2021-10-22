using DB.Data;
using DB.Entities;
using Generic.Extensions;
using Microsoft.EntityFrameworkCore;
using DTO.DTOs;
using Service.Helpers;
using Service.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Service.BOs;

namespace Service.Repositories
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext context;

        public LikesRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int likeUserId)
        {
            return await context.Likes.FindAsync(sourceUserId, likeUserId);
        }

        /// <summary>
        /// Récupération des éléments qui ont été like
        /// En fonction de quel string est dans la requête --> (liked/likedBy)
        /// --> On retourne une liste différente 
        /// </summary>
        public async Task<PagedList<LikeBo>> GetUserLikes(LikesParams likesParams)
        {
            IQueryable<AppUser> users = context.Users.OrderBy(u => u.UserName).AsQueryable();
            IQueryable<UserLike> likes = context.Likes.AsQueryable();

            if (likesParams.Predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.LikedUser);
            }
            if (likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(like => like.LikedUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }
            IQueryable<LikeBo> likedUsers = users.Select(user => new LikeBo
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Id = user.Id
            });

            return await PagedList<LikeBo>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await context.Users
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
