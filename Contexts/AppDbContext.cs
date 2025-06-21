using Microsoft.EntityFrameworkCore;
using StatSanctum.Entities;

namespace StatSanctum.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Equipment> Equipments { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
