using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Work_With_Jwt.Data.Dtos;
using Work_With_Jwt.Models;
using Work_With_Jwt.Services.Interfaces;

namespace Work_With_Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public static User user = new User();
        private readonly IUserServices _userServices;
        private readonly ITokenService _tokenService;

        public AuthenticationController(IUserServices userServices, ITokenService tokenService)
        {
            _userServices = userServices;
            _tokenService = tokenService;

        }

        [HttpGet, Authorize]
        public ActionResult<string> GetName()
        {
            var userName = _userServices.GetUsersName();
            return Ok(userName);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {

            _userServices.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return Ok(user);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            if (user.Username != request.Username)
            {
                return BadRequest(" User not found");
            }

            if (!_userServices.VerfiyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Oops Wrong Password...");
            }

            string token = _tokenService.CreateToken(user);
            if (token == null) { return Unauthorized(); }

            var refreshToken = _tokenService.GenerateRefreshToken();
            _tokenService.SetRefreshToken(refreshToken, user);

            return Ok(token);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token... !");
            }
            else if (user.TokenExpiresTime < DateTime.Now)
            {
                return Unauthorized("Token Expired...!");
            }
            var token = _tokenService.CreateToken(user);
            var NewrefreshToken = _tokenService.GenerateRefreshToken();
            _tokenService.SetRefreshToken(NewrefreshToken,user);

            return Ok(token);
        }
    }
}
