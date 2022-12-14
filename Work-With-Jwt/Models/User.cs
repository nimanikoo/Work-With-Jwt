namespace Work_With_Jwt.Models
{
    public class User
    {
        public string Username { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }

        public string RefreshToken { get; set; }
        public DateTime TokenCreationTime { get; set; }
        public DateTime TokenExpiresTime { get; set; }
    }
}
