using LoginQRCode.Dtos;
using LoginQRCode.Entities;
using LoginQRCode.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginQRCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null)
            {
                ModelState.AddModelError("username", "Invalid Username");
                return ValidationProblem();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("password", "Invalid password");
                return ValidationProblem();
            }

            return new UserDto
            {
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                Token = await _tokenService.CreateTokenAsync(user),
            };
        }

        [HttpPost("login-signalr/{username}")]
        public async Task<ActionResult<UserDto>> Login(string username)
        {
            var user = await _userManager.Users
                .SingleOrDefaultAsync(x => x.UserName == username);

            if (user == null)
            {
                ModelState.AddModelError("username", "Invalid Username");
                return ValidationProblem();
            }
            
            return new UserDto
            {
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                Token = await _tokenService.CreateTokenAsync(user),
            };
        }
    }
}
