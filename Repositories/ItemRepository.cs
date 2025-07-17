using Microsoft.EntityFrameworkCore;
using StatSanctum.Contexts;
using StatSanctum.Entities;
using StatSanctum.Models;

namespace StatSanctum.Repositories
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<ItemRarityDto>> GetAllWithRarityAsync()
        {
            return await _context.Item
                .Include(i => i.Rarity)
                .Select(i => new ItemRarityDto
                {
                    ItemID = i.ItemID,
                    Name = i.Name,
                    Description = i.Description,
                    Level = i.Level,
                    Rarity = new RarityDto
                    {
                        Name = i.Rarity.Name,
                        Image = i.Rarity.Image
                    }
                })
                .ToListAsync();
        }

        public async Task<ItemRarityDto> GetByIdWithRarityAsync(int id)
        {
            return await _context.Item
                .Include(i => i.Rarity)
                .Select(i => new ItemRarityDto
                {
                    ItemID = i.ItemID,
                    Name = i.Name,
                    Description = i.Description,
                    Level = i.Level,
                    Rarity = new RarityDto
                    {
                        Name = i.Rarity.Name,
                        Image = i.Rarity.Image
                    }
                })
                .FirstOrDefaultAsync(i => i.ItemID == id)
                ?? throw new KeyNotFoundException($"{typeof(Item)} with ID {id} not found");
        }
    }
}
