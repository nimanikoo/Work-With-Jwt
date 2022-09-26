using Work_With_Jwt.Models;

namespace Work_With_Jwt.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
        RefresToken GenerateRefreshToken();
        void SetRefreshToken(RefresToken refreshToken,User user);
    }
}
