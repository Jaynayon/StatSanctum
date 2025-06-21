using Microsoft.EntityFrameworkCore;
using StatSanctum.Contexts;
using StatSanctum.Entities;
using StatSanctum.Models;

namespace StatSanctum.Repositories
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly AppDbContext _context;
        public EquipmentRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Equipment> CreateEquipment(EquipmentDto equipment)
        {
            if (equipment == null)
            {
                throw new ArgumentNullException(nameof(equipment));
            }

            var newEquipment = new Equipment
            {
                Name = equipment.Name,
                Description = equipment.Description ?? "",
                Type = equipment.Type,
                Level = equipment.Level
            };

            await _context.Equipments.AddAsync(newEquipment);
            await _context.SaveChangesAsync(); // Save to DB

            return newEquipment;
        }

        public async Task<IEnumerable<Equipment>> GetAllEquipments()
        {
            return await _context.Equipments.ToListAsync();
        }

        public async Task<Equipment> GetEquipmentById(int id)
        {
            var equipment = await _context.Equipments.FindAsync(id);
            if (equipment == null)
            {
                throw new KeyNotFoundException($"Equipment with ID {id} not found.");
            }
            return equipment;
        }

        public async Task<Equipment> UpdateEquipment(int id, EquipmentDto equipmentDto)
        {
            if (equipmentDto == null)
            {
                throw new ArgumentNullException(nameof(equipmentDto));
            }

            var equipment = await GetEquipmentById(id);
           
            // Update properties
            equipment.Name = equipmentDto.Name;
            equipment.Description = equipmentDto.Description ?? "";
            equipment.Level = equipmentDto.Level;
            equipment.Type = equipmentDto.Type;
            _context.Equipments.Update(equipment);
            await _context.SaveChangesAsync(); // Save changes to DB
            return equipment;
        }

        public async Task DeleteEquipmentById(int id)
        {
            var equipment = await GetEquipmentById(id);
            if (equipment == null)
            {
                throw new KeyNotFoundException($"Equipment with ID {id} not found.");
            }
            _context.Equipments.Remove(equipment);
            await _context.SaveChangesAsync(); // Save changes to DB
        }
    }
}
