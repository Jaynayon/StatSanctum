using MediatR;
using StatSanctum.Entities;
using StatSanctum.Models;
using StatSanctum.Queries.Items;
using StatSanctum.Repositories;

namespace StatSanctum.Handlers
{
    public class ItemHandler :
        IRequestHandler<GetAllItemsQuery, IEnumerable<ItemRarityDto>>,
        IRequestHandler<GetItemByIdQuery, ItemRarityDto>
    {
        private readonly IItemRepository _itemRepository;

        public ItemHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IEnumerable<ItemRarityDto>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
        {
            return await _itemRepository.GetAllWithRarityAsync();
        }

        public async Task<ItemRarityDto> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            return await _itemRepository.GetByIdWithRarityAsync(request.Id);
        }
    }
}
