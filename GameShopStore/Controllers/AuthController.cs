using GameShopStore.Core.Dtos.UserDtos;
using GameShopStore.Core.Models;
using GameShopStore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameShopStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IJwtTokenHelper _jwtTokenHelper;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public AuthController(IConfiguration config,
            UserManager<User> userManager, SignInManager<User> signInManager, IJwtTokenHelper jwtTokenHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _jwtTokenHelper = jwtTokenHelper;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userForRegisterDto)
        {
            var userToCreate = new User
            {
                UserName = userForRegisterDto.Username,
                SurName = userForRegisterDto.Surname,
                Email = userForRegisterDto.Email,
                PhoneNumber = userForRegisterDto.Phone
            };

            var result = await _userManager.CreateAsync(userToCreate, userForRegisterDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            result = await _userManager.AddToRoleAsync(userToCreate, "Customer");

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userForLoginDto)
        {

            var user = await _userManager.FindByEmailAsync(userForLoginDto.Email);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    token = await _jwtTokenHelper.GenerateJwtToken(user, _userManager, _config)
                });
            }


            return Unauthorized();

        }
    }
}
