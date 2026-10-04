using AutoMapper;
using MicroserviceApp.Common.Abstractions.Database;
using MicroserviceApp.Inventory.Application.Dto;
using MicroserviceApp.Inventory.Application.RequestHandlers;
using MicroserviceApp.Inventory.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace MicroserviceApp.Inventory.Application
{
    public interface IInventoryRepository
    {
        Task<InventoryItemDto> GetInventoryItemAsync(Guid productId);
        Task<InventoryItemDto> AdjustStockAsync(AdjustStock request);
        Task<InventoryReservationDto> ReserveInventoryAsync(ReserveInventory request);
    }

    public class InventoryRepository : IInventoryRepository
    {
        private IExtendedDbProvider<InventoryItem> _itemDbProvider { get; }
        private IExtendedDbProvider<InventoryReservation> _reservationDbProvider { get; }
        public IConfiguration _configuration { get; }
        public IMapper _mapper { get; }

        public InventoryRepository(
            IExtendedDbProvider<InventoryItem> itemDbProvider,
            IExtendedDbProvider<InventoryReservation> reservationDbProvider,
            IConfiguration configuration,
            IMapper mapper)
        {
            _itemDbProvider = itemDbProvider;
            _reservationDbProvider = reservationDbProvider;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<InventoryItemDto> GetInventoryItemAsync(Guid productId)
        {
            var item = await _itemDbProvider.GetItemAsync(productId.ToString());
            return _mapper.Map<InventoryItemDto>(item);
        }

        public async Task<InventoryItemDto> AdjustStockAsync(AdjustStock request)
        {
            var item = new InventoryItem
            {
                Id = request.ProductId,
                ProductId = request.ProductId,
                ProductName = request.ProductName,
                QuantityAvailable = request.Quantity
            };

            var existing = await _itemDbProvider.GetItemAsync(request.ProductId.ToString());
            var saved = existing == null
                ? await _itemDbProvider.CreateItemAsync(item)
                : await _itemDbProvider.UpdateItemAsync(request.ProductId.ToString(), item);

            return saved ? _mapper.Map<InventoryItemDto>(item) : null;
        }

        public async Task<InventoryReservationDto> ReserveInventoryAsync(ReserveInventory request)
        {
            var reservation = new InventoryReservation
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderId,
                Products = _mapper.Map<IEnumerable<Product>>(request.Products),
                IsReserved = request.Products?.Any() ?? false
            };

            var isAdded = await _reservationDbProvider.CreateItemAsync(reservation);
            return isAdded ? _mapper.Map<InventoryReservationDto>(reservation) : null;
        }
    }
}
