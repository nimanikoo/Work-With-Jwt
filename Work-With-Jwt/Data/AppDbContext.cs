using Authentication_with_Jwt.Models;
using Microsoft.EntityFrameworkCore;

namespace Work_With_Jwt.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Team> Teams { get; set; }

    }
}
