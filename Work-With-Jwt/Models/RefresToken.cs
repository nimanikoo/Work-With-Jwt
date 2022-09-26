namespace Work_With_Jwt.Models
{
    public class RefresToken
    {
        public string Token { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime ExpireTime { get; set; }
    }
}
