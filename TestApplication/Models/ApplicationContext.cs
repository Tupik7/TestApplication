using Microsoft.EntityFrameworkCore;
using TestApplication.Models;

namespace TestApplication
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
