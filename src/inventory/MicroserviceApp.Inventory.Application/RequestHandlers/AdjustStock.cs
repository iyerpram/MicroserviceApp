using MediatR;
using MicroserviceApp.Inventory.Application.Dto;

namespace MicroserviceApp.Inventory.Application.RequestHandlers
{
    public class AdjustStock : IRequest<InventoryItemDto>
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
    }

    public class AdjustStockHandler : IRequestHandler<AdjustStock, InventoryItemDto>
    {
        public IInventoryRepository _inventoryRepository { get; }

        public AdjustStockHandler(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<InventoryItemDto> Handle(AdjustStock request, CancellationToken cancellationToken)
        {
            if (request.ProductId == Guid.Empty)
                return null;

            return await _inventoryRepository.AdjustStockAsync(request);
        }
    }
}
