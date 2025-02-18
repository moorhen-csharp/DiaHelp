using DiaHelp.Model;
using Microsoft.EntityFrameworkCore;

namespace DiaHelp.Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<SugarModel> SugarNotes => Set<SugarModel>();
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) => Database.EnsureCreated();
    }
}
