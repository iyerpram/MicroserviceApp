using MediatR;
using MicroserviceApp.Common.Abstractions.Messaging;
using MicroserviceApp.Common.Application;
using MicroserviceApp.Common.Application.Events;

namespace MicroserviceApp.Orders.Application.RequestHandlers
{
    public class CreateOrder: IRequest<OrderDto>
    {
        public UserDto? User { get; set; }
        public IEnumerable<ProductDto>? Products { get; set; }
    }

    public class CreateOrderHandler : IRequestHandler<CreateOrder, OrderDto>
    {
        public IOrderRepository _orderRepository { get; }
        public IMessagingProviderFactory _messagingProviderFactory { get; }

        public CreateOrderHandler(IOrderRepository orderRepository, IMessagingProviderFactory messagingProviderFactory)
        {
            _orderRepository = orderRepository;
            _messagingProviderFactory = messagingProviderFactory;
        }        

        public async Task<OrderDto> Handle(CreateOrder request, CancellationToken cancellationToken)
        {
            if (request?.User == null)
                return null;

            var order = await _orderRepository.CreateOrderAsync(request);
            if (order == null)
                return null;

            var orderCreated = new OrderCreated
            {
                OrderId = order.Id,
                User = order.User,
                Products = order.Products,
                TotalAmount = order.Products?.Sum(p => p.Price) ?? 0
            };

            var paymentProvider = _messagingProviderFactory.GetMessagingProvider(MessagingProviderType.AzureServiceBus, "Payment");
            await paymentProvider.PublishMessageAsync("Order Created", orderCreated);

            var notificationProvider = _messagingProviderFactory.GetMessagingProvider(MessagingProviderType.AzureServiceBus, "Notification");
            await notificationProvider.PublishMessageAsync("Notification Requested", new NotificationRequested
            {
                UserId = request.User.Id,
                Email = request.User.Email,
                EventType = nameof(OrderCreated),
                RelatedEntityId = order.Id,
                Subject = "Order created",
                Body = $"Order {order.Id} was created."
            });

            var cartProvider = _messagingProviderFactory.GetMessagingProvider(MessagingProviderType.AWS_SNS, "Cart");
            await cartProvider.PublishMessageAsync("Order Creation Successfull", order);

            return order;
        }
    }
}
