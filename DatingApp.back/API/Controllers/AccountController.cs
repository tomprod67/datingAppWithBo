using System.Linq;
using System.Threading.Tasks;
using Service.DTOs;
using DB.Entities;
using Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using DTO.DTOs;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        private readonly IAccountService accountService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper, IAccountService accountService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
            this.accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            AppUser user = mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.Username.ToLower();

            IdentityResult result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            IdentityResult roleResult = await userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) 
                return BadRequest(result.Errors);

            UserDto returnUserDto = accountService.RegisterUser(user.UserName, tokenService.CreateToken(user), user.KnownAs, user.Gender);
            return returnUserDto;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            AppUser user = await userManager.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) 
                return Unauthorized("Invalid username");

            SignInResult result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) 
                return Unauthorized();

            UserDto returnUserDto = accountService.LoginUser(user.UserName, tokenService.CreateToken(user), user.Photos.FirstOrDefault(x => x.IsMain)?.Url, user.KnownAs, user.Gender);
            return returnUserDto;
        }

        private async Task<bool> UserExists(string username)
        {
            return await userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}