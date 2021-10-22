using Generic.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using DTO.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Service.Helpers;
using Service.Extensions;
using DB.Entities;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILikesService likesService;

        public LikesController(IUnitOfWork unitOfWork, ILikesService likesService)
        {
            this.unitOfWork = unitOfWork;
            this.likesService = likesService;
        }

        /// <summary>
        /// Permet l'ajout de like vers une personne
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            int sourceUserId = User.GetUserId();
            AppUser likeUser = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            AppUser sourceUser = await unitOfWork.LikesRepository.GetUserWithLikes(sourceUserId);

            if (likeUser == null) 
                return NotFound();

            if (sourceUser.UserName == username) 
                return BadRequest("You cannot like youselft");

            UserLike userLike = await unitOfWork.LikesRepository.GetUserLike(sourceUserId, likeUser.Id);

            if (userLike != null) 
                return BadRequest("You already like this user");

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likeUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);

            if (await unitOfWork.Complete())
                return Ok();

            return BadRequest("Failed to like user");
        }


        /// <summary>
        /// Récupération de tout les likes d'un personne / ou les personnes qui l'on like et ainsi organiser la réponse en pagination
        /// </summary>
        /// <param name="likesParams"></param>
        /// <returns>Retour une liste d'utilisateur qui ont/qu'il a like </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            PagedList<LikeDto> users = likesService.GetUserLikes(likesParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }
    }
}
