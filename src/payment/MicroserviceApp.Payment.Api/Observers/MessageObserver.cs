using MediatR;
using MicroserviceApp.Common.Abstractions.Messaging;
using MicroserviceApp.Common.Application.Events;
using MicroserviceApp.Payment.Application.RequestHandlers;
using System.Text.Json;

namespace MicroserviceApp.Payment.Api.Observers
{
    public class MessageObserver : IHostedService
    {
        private Timer _timer;
        public IMessagingProvider _messagingProvider { get; }
        public IMediator _mediator { get; }
        public ILogger<MessageObserver> _logger { get; }

        public MessageObserver(IMessagingProvider messagingProvider, IMediator mediator, ILogger<MessageObserver> logger)
        {
            _messagingProvider = messagingProvider;
            _mediator = mediator;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ReadMessage, null, 0, 10000);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        void ReadMessage(object state)
        {
            _messagingProvider.SubscribeMessageAsync<OrderCreated>(ProcessOrderCreated);
        }

        async Task ProcessOrderCreated(OrderCreated order)
        {
            if (order == null || order.OrderId == Guid.Empty || order.User == null)
            {
                _logger.LogWarning($"Invalid order created event received. Request: {JsonSerializer.Serialize(order)}");
                return;
            }

            var response = await _mediator.Send(new ProcessPayment
            {
                OrderId = order.OrderId,
                User = order.User,
                Products = order.Products,
                Amount = order.TotalAmount
            });

            if (response == null)
                _logger.LogWarning($"Payment processing failed for order {order.OrderId}");
            else
                _logger.LogInformation($"Payment processing successfull, payment id: {response.Id}");
        }
    }
}
