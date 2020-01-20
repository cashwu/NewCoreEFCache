using Microsoft.EntityFrameworkCore;

namespace testEFCoreCacheable.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) 
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}