using MediatR;
using MicroserviceApp.Common.Abstractions.Messaging;
using MicroserviceApp.Common.Application;

namespace MicroserviceApp.Cart.Application.RequestHandlers
{
    public class CreateOrder: IRequest<bool>
    {
        public UserDto? User { get; set; }
        public IEnumerable<ProductDto>? Products { get; set; }
    }

    public class CreateOrderHandler : IRequestHandler<CreateOrder, bool>
    {
        public IMessagingProviderFactory _messagingProviderFactory { get; }

        public CreateOrderHandler(IMessagingProviderFactory messagingProviderFactory)
        {
            _messagingProviderFactory = messagingProviderFactory;
        }        

        public async Task<bool> Handle(CreateOrder request, CancellationToken cancellationToken)
        {
            if (request?.User == null || (request?.Products?.Any() ?? true))
                return false;

            var messagingProvider = _messagingProviderFactory.GetMessagingProvider(MessagingProviderType.AzureServiceBus, "Order");
            return await messagingProvider.PublishMessageAsync("Cart Checkout", request);
        }
    }
}
