using MediatR;
using StatSanctum.Entities;
using StatSanctum.Models;

namespace StatSanctum.Queries.Items
{
    public class GetItemByIdQuery : IRequest<ItemRarityDto>
    {
        public int Id { get; set; }
    }
}
