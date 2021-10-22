using DB.Data;
using DB.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        /// <summary>
        /// Controller permettant une gestion des erreurs les plus communes
        /// </summary>
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Error 401 : Problème d'authentification
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        /// <summary>
        /// Error 404
        /// </summary>
        /// <returns></returns>
        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.Users.Find(-1);

            if (thing == null) return NotFound();

            return Ok(thing);
        }

        /// <summary>
        /// Error 500
        /// </summary>
        /// <returns></returns>
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _context.Users.Find(-1);

            var thingToReturn = thing.ToString();

            return thingToReturn;
        }

        /// <summary>
        /// Error 400
        /// </summary>
        /// <returns></returns>
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request");
        }
    }
}