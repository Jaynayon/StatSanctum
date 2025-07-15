using StatSanctum.Entities;
using StatSanctum.Models;

namespace StatSanctum.Repositories
{
    public interface IRarityRepository : IRepository<RarityDto, Rarity>
    {
        // Implements all IRepository contract as well as allow abstract methods for Rarity
    }
}
