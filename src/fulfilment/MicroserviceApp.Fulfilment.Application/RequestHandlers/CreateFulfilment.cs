using MediatR;
using MicroserviceApp.Common.Abstractions.Messaging;
using MicroserviceApp.Common.Application;
using MicroserviceApp.Common.Application.Events;
using MicroserviceApp.Fulfilment.Application.Dto;

namespace MicroserviceApp.Fulfilment.Application.RequestHandlers
{
    public class CreateFulfilment : IRequest<FulfilmentDto>
    {
        public Guid OrderId { get; set; }
        public UserDto? User { get; set; }
    }

    public class CreateFulfilmentHandler : IRequestHandler<CreateFulfilment, FulfilmentDto>
    {
        public IFulfilmentRepository _fulfilmentRepository { get; }
        public IMessagingProviderFactory _messagingProviderFactory { get; }

        public CreateFulfilmentHandler(IFulfilmentRepository fulfilmentRepository, IMessagingProviderFactory messagingProviderFactory)
        {
            _fulfilmentRepository = fulfilmentRepository;
            _messagingProviderFactory = messagingProviderFactory;
        }

        public async Task<FulfilmentDto> Handle(CreateFulfilment request, CancellationToken cancellationToken)
        {
            if (request.OrderId == Guid.Empty || request.User == null)
                return null;

            var fulfilment = await _fulfilmentRepository.CreateFulfilmentAsync(request);
            if (fulfilment == null)
                return null;

            await PublishFulfilmentUpdated(fulfilment);
            return fulfilment;
        }

        private async Task PublishFulfilmentUpdated(FulfilmentDto fulfilment)
        {
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
                Subject = "Fulfilment updated",
                Body = $"Fulfilment for order {fulfilment.OrderId} is {fulfilment.Status}."
            });
        }
    }
}
