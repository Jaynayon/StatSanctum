using StatSanctum.Entities;
using StatSanctum.Models;

namespace StatSanctum.Repositories
{
    public interface IItemRepository : IRepository<Item>
    {
        // Item-specific methods
        Task<ItemRarityDto> GetByIdWithRarityAsync(int id);
        Task<IEnumerable<ItemRarityDto>> GetAllWithRarityAsync();
    }
}
