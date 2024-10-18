using Microsoft.EntityFrameworkCore;
using ProgPoe.Models;
namespace ProgPoe.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
        DbSet<userTable> userTable { get; set; }
    }

    /*
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
    */
}
