using Microsoft.EntityFrameworkCore;
using StatSanctum.Contexts;
using StatSanctum.Entities;
using StatSanctum.Models;

namespace StatSanctum.Repositories
{
    public class RarityRepository : IRarityRepository
    {
        private readonly AppDbContext _context;
        public RarityRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Rarity> Create(RarityDto rarity)
        {
            if (rarity == null)
            {
                throw new ArgumentNullException(nameof(rarity));
            }

            var newRarity = new Rarity
            {
                Name = rarity.Name,
                Image = rarity.Image ?? ""
            };

            await _context.Rarity.AddAsync(newRarity);
            await _context.SaveChangesAsync(); // Save to DB

            return newRarity;
        }

        public async Task DeleteById(int id)
        {
            var rarity = await GetById(id);
            if (rarity == null)
            {
                throw new KeyNotFoundException($"Equipment with ID {id} not found.");
            }
            _context.Rarity.Remove(rarity);
            await _context.SaveChangesAsync(); // Save changes to DB
        }

        public async Task<IEnumerable<Rarity>> GetAll()
        {
            return await _context.Rarity.ToListAsync();
        }

        public async Task<Rarity> GetById(int id)
        {
            var rarity = await _context.Rarity.FindAsync(id);
            if (rarity == null)
            {
                throw new KeyNotFoundException($"Rarity with ID {id} not found.");
            }
            return rarity;
        }

        public async Task<Rarity> Update(int id, RarityDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var rarity = await GetById(id);

            // Update properties
            rarity.Name = dto.Name;
            rarity.Image = dto.Image ?? "";
            _context.Rarity.Update(rarity);
            await _context.SaveChangesAsync(); // Save changes to DB
            return rarity;
        }
    }
}
