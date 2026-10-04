using MediatR;
using MicroserviceApp.Common.Abstractions.Messaging;
using MicroserviceApp.Common.Application.Events;
using MicroserviceApp.Inventory.Application.RequestHandlers;
using System.Text.Json;

namespace MicroserviceApp.Inventory.Api.Observers
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
            _messagingProvider.SubscribeMessageAsync<PaymentProcessed>(ProcessPaymentProcessed);
        }

        async Task ProcessPaymentProcessed(PaymentProcessed payment)
        {
            if (payment == null || payment.OrderId == Guid.Empty || !string.Equals(payment.Status, "Completed", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning($"Inventory reservation skipped. Request: {JsonSerializer.Serialize(payment)}");
                return;
            }

            var response = await _mediator.Send(new ReserveInventory
            {
                OrderId = payment.OrderId,
                User = payment.User,
                Products = payment.Products
            });

            if (response == null)
                _logger.LogWarning($"Inventory reservation failed for order {payment.OrderId}");
            else
                _logger.LogInformation($"Inventory reservation successfull, reservation id: {response.Id}");
        }
    }
}
