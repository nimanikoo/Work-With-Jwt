using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Work_With_Jwt.Models;
using Work_With_Jwt.Services.Interfaces;

namespace Work_With_Jwt.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        public TokenService(IConfiguration configuration,IHttpContextAccessor contextAccessor)
        {
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }
        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
             {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
             };

            var tokenKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Key").Value));

            var tokenCredential = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: tokenCredential
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public RefresToken GenerateRefreshToken()
        {
            var refreshToken = new RefresToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.Now,
                ExpireTime = DateTime.Now.AddDays(5)
            };
            return refreshToken;
        }

        public void SetRefreshToken(RefresToken refreshToken,User user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.ExpireTime,
            };
            _contextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
            user.RefreshToken = refreshToken.Token;
            user.TokenCreationTime = refreshToken.Created;
            user.TokenExpiresTime = refreshToken.ExpireTime;
        }
    }
}
