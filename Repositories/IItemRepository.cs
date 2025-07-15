using StatSanctum.Entities;
using StatSanctum.Models;

namespace StatSanctum.Repositories
{
    public interface IItemRepository : IRepository<ItemDto, Item>
    {
        //Task<IEnumerable<Equipment>> GetAllEquipments();
        //Task<Equipment> CreateEquipment(EquipmentDto equipment);
        //Task<Equipment> GetEquipmentById(int id);
        //Task DeleteEquipmentById(int id);
        //Task<Equipment> UpdateEquipment(int id, EquipmentDto equipmentDto);
    }
}
