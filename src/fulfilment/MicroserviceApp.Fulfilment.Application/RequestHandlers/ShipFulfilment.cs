using MediatR;
using MicroserviceApp.Common.Abstractions.Messaging;
using MicroserviceApp.Common.Application.Events;
using MicroserviceApp.Fulfilment.Application.Dto;

namespace MicroserviceApp.Fulfilment.Application.RequestHandlers
{
    public class ShipFulfilment : IRequest<FulfilmentDto>
    {
        public Guid FulfilmentId { get; set; }
        public string? TrackingNumber { get; set; }
    }

    public class ShipFulfilmentHandler : IRequestHandler<ShipFulfilment, FulfilmentDto>
    {
        public IFulfilmentRepository _fulfilmentRepository { get; }
        public IMessagingProviderFactory _messagingProviderFactory { get; }

        public ShipFulfilmentHandler(IFulfilmentRepository fulfilmentRepository, IMessagingProviderFactory messagingProviderFactory)
        {
            _fulfilmentRepository = fulfilmentRepository;
            _messagingProviderFactory = messagingProviderFactory;
        }

        public async Task<FulfilmentDto> Handle(ShipFulfilment request, CancellationToken cancellationToken)
        {
            if (request.FulfilmentId == Guid.Empty)
                return null;

            var fulfilment = await _fulfilmentRepository.ShipFulfilmentAsync(request);
            if (fulfilment == null)
                return null;

            var fulfilmentUpdated = new FulfilmentUpdated
            {
                FulfilmentId = fulfilment.Id,
                OrderId = fulfilment.OrderId,
                User = fulfilment.User,
                Status = fulfilment.Status,
                TrackingNumber = fulfilment.TrackingNumber
            };

            var ordersProvider = _messagingProviderFactory.GetMessagingProvider(MessagingProviderType.AzureServiceBus, "Order");
            await ordersProvider.PublishMessageAsync("Fulfilment Updated", fulfilmentUpdated);

            var notificationProvider = _messagingProviderFactory.GetMessagingProvider(MessagingProviderType.AzureServiceBus, "Notification");
            await notificationProvider.PublishMessageAsync("Notification Requested", new NotificationRequested
            {
                UserId = fulfilment.User?.Id ?? 0,
                Email = fulfilment.User?.Email,
                EventType = nameof(FulfilmentUpdated),
                RelatedEntityId = fulfilment.Id,
                Subject = "Order shipped",
                Body = $"Order {fulfilment.OrderId} shipped with tracking {fulfilment.TrackingNumber}."
            });

            return fulfilment;
        }
    }
}
