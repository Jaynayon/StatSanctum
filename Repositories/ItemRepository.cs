using Microsoft.EntityFrameworkCore;
using StatSanctum.Contexts;
using StatSanctum.Entities;
using StatSanctum.Models;

namespace StatSanctum.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;
        public ItemRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Item>> GetAll()
        {
            return await _context.Item.ToListAsync();
        }

        public async Task<Item> Create(ItemDto equipment)
        {
            if (equipment == null)
            {
                throw new ArgumentNullException(nameof(equipment));
            }

            var newEquipment = new Item
            {
                Name = equipment.Name,
                Description = equipment.Description ?? "",
                Type = equipment.Type,
                Level = equipment.Level
            };

            await _context.Item.AddAsync(newEquipment);
            await _context.SaveChangesAsync(); // Save to DB

            return newEquipment;
        }

        public async Task<Item> GetById(int id)
        {
            var equipment = await _context.Item.FindAsync(id);
            if (equipment == null)
            {
                throw new KeyNotFoundException($"Equipment with ID {id} not found.");
            }
            return equipment;
        }

        public async Task DeleteById(int id)
        {
            var equipment = await GetById(id);
            if (equipment == null)
            {
                throw new KeyNotFoundException($"Equipment with ID {id} not found.");
            }
            _context.Item.Remove(equipment);
            await _context.SaveChangesAsync(); // Save changes to DB
        }

        public async Task<Item> Update(int id, ItemDto equipmentDto)
        {
            if (equipmentDto == null)
            {
                throw new ArgumentNullException(nameof(equipmentDto));
            }

            var equipment = await GetById(id);

            // Update properties
            equipment.Name = equipmentDto.Name;
            equipment.Description = equipmentDto.Description ?? "";
            equipment.Level = equipmentDto.Level;
            equipment.Type = equipmentDto.Type;
            _context.Item.Update(equipment);
            await _context.SaveChangesAsync(); // Save changes to DB
            return equipment;
        }
    }
}
