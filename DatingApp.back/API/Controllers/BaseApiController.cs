using Microsoft.AspNetCore.Mvc;
using Service.Helpers;

namespace API.Controllers
{
    /// <summary>
    /// C'est le controlleur qui permet de uniformiser les routes de tout les controlleurs qui en herite avec un redirection vers --> api/nom du controller
    /// </summary>
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        
    }
}