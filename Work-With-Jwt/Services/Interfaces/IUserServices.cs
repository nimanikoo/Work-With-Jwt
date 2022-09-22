namespace Work_With_Jwt.Services.Interfaces
{
    public interface IUserServices
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerfiyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        
    }
}
