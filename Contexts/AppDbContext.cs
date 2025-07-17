using Microsoft.EntityFrameworkCore;
using StatSanctum.Entities;

namespace StatSanctum.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Item> Item { get; set; }
        public DbSet<Rarity> Rarity { get; set; }
        public DbSet<ItemType> ItemType { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().ToTable("MST_Item"); // Specify the table name
            modelBuilder.Entity<Rarity>().ToTable("LKP_Rarity");
            modelBuilder.Entity<ItemType>().ToTable("LKP_ItemType");
        }
    }
}
