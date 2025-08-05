using MediatR;
using StatSanctum.Entities;
using StatSanctum.Models;

namespace StatSanctum.Queries.Items
{
    public class GetAllItemsQuery : IRequest<IEnumerable<ItemRarityDto>>
    {
    }
}
