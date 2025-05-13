using DiaHelp.Model;
using Microsoft.EntityFrameworkCore;

namespace DiaHelp.Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public DbSet<UserModel> Users => Set<UserModel>();
        public DbSet<SugarModel> SugarNotes => Set<SugarModel>();
        public DbSet<FoodModel> FoodNotes => Set<FoodModel>();
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) => Database.EnsureCreated();
    }
}
