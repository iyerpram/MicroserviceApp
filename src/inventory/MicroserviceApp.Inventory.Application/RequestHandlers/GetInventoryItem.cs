using MediatR;
using MicroserviceApp.Inventory.Application.Dto;

namespace MicroserviceApp.Inventory.Application.RequestHandlers
{
    public class GetInventoryItem : IRequest<InventoryItemDto>
    {
        public Guid ProductId { get; set; }
    }

    public class GetInventoryItemHandler : IRequestHandler<GetInventoryItem, InventoryItemDto>
    {
        public IInventoryRepository _inventoryRepository { get; }

        public GetInventoryItemHandler(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<InventoryItemDto> Handle(GetInventoryItem request, CancellationToken cancellationToken)
        {
            if (request.ProductId == Guid.Empty)
                return null;

            return await _inventoryRepository.GetInventoryItemAsync(request.ProductId);
        }
    }
}
