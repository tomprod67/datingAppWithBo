using DTO.DTOs;
using Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ILikesService
    {
        PagedList<LikeDto> GetUserLikes(LikesParams likesParams);
    }
}
