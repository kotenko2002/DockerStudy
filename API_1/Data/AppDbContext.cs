using Microsoft.EntityFrameworkCore;

namespace API_1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Ping> Pings { get; set; }
    }

    public class Ping
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string ReciverName { get; set; }
    }
}
