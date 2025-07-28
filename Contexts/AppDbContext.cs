using Microsoft.EntityFrameworkCore;
using StatSanctum.Entities;

namespace StatSanctum.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Item> Item { get; set; }
        public DbSet<Rarity> Rarity { get; set; }
        public DbSet<ItemType> ItemType { get; set; }
        public DbSet<User> User { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().ToTable("MST_Item");
            modelBuilder.Entity<Rarity>().ToTable("LKP_Rarity");
            modelBuilder.Entity<ItemType>().ToTable("LKP_ItemType");
            modelBuilder.Entity<User>().ToTable("MST_User");
        }
    }
}
