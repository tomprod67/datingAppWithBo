using Generic.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Service.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Service.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {

        /// <summary>
        /// Permet de récupérer les informations générale de la sessions qu'on va réutiller par la suite dans les autres méthodes
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ActionExecutedContext resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) 
                return;

            int userId = resultContext.HttpContext.User.GetUserId();
            IUnitOfWork unitOfWork = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();
            DB.Entities.AppUser user = await unitOfWork.UserRepository.GetUserByIdAsync(userId);
            user.LastActive = DateTime.Now;
            await unitOfWork.Complete();
        }
    }
}
