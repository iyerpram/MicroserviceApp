using MediatR;
using MicroserviceApp.Common.Abstractions.Messaging;
using MicroserviceApp.Common.Application;
using MicroserviceApp.Common.Application.Events;
using MicroserviceApp.Inventory.Application.Dto;

namespace MicroserviceApp.Inventory.Application.RequestHandlers
{
    public class ReserveInventory : IRequest<InventoryReservationDto>
    {
        public Guid OrderId { get; set; }
        public UserDto? User { get; set; }
        public IEnumerable<ProductDto>? Products { get; set; }
    }

    public class ReserveInventoryHandler : IRequestHandler<ReserveInventory, InventoryReservationDto>
    {
        public IInventoryRepository _inventoryRepository { get; }
        public IMessagingProviderFactory _messagingProviderFactory { get; }

        public ReserveInventoryHandler(IInventoryRepository inventoryRepository, IMessagingProviderFactory messagingProviderFactory)
        {
            _inventoryRepository = inventoryRepository;
            _messagingProviderFactory = messagingProviderFactory;
        }

        public async Task<InventoryReservationDto> Handle(ReserveInventory request, CancellationToken cancellationToken)
        {
            if (request.OrderId == Guid.Empty || (!request.Products?.Any() ?? true))
                return null;

            var reservation = await _inventoryRepository.ReserveInventoryAsync(request);
            if (reservation == null)
                return null;

            var inventoryReserved = new InventoryReserved
            {
                ReservationId = reservation.Id,
                OrderId = reservation.OrderId,
                User = request.User,
                Products = reservation.Products,
                Success = reservation.IsReserved
            };

            var fulfilmentProvider = _messagingProviderFactory.GetMessagingProvider(MessagingProviderType.AzureServiceBus, "Fulfilment");
            await fulfilmentProvider.PublishMessageAsync("Inventory Reserved", inventoryReserved);

            var notificationProvider = _messagingProviderFactory.GetMessagingProvider(MessagingProviderType.AzureServiceBus, "Notification");
            await notificationProvider.PublishMessageAsync("Notification Requested", new NotificationRequested
            {
                UserId = request.User?.Id ?? 0,
                Email = request.User?.Email,
                EventType = nameof(InventoryReserved),
                RelatedEntityId = reservation.Id,
                Subject = "Inventory reserved",
                Body = $"Inventory for order {reservation.OrderId} reserved: {reservation.IsReserved}."
            });

            return reservation;
        }
    }
}
