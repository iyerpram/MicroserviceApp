using MediatR;
using MicroserviceApp.Common.Abstractions.Messaging;
using MicroserviceApp.Common.Application.Events;
using MicroserviceApp.Orders.Application.RequestHandlers;
using System.Text.Json;

namespace MicroserviceApp.Orders.Api.Observers
{
    public class MessageObserver : IHostedService
    {
        private Timer _timer;
        public IMessagingProvider _messagingProvider { get; }
        public IMediator _mediator { get; }
        public ILogger<MessageObserver> _logger { get; }

        public MessageObserver(IMessagingProvider messagingProvider, IMediator mediator
            , ILogger<MessageObserver> logger) 
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
            _messagingProvider.SubscribeMessageAsync<CreateOrder>(ProcessCreateOrder);
            _messagingProvider.SubscribeMessageAsync<PaymentProcessed>(ProcessPaymentProcessed);
            _messagingProvider.SubscribeMessageAsync<FulfilmentUpdated>(ProcessFulfilmentUpdated);
        }

        async Task ProcessCreateOrder(CreateOrder request)
        {
            if (request?.User == null || (!request?.Products?.Any() ?? true))
                _logger.LogWarning($"Invalid order creation request received. Request: {JsonSerializer.Serialize(request)}");

            var response = await _mediator.Send(request);
            if (response == null)
                _logger.LogWarning($"Order creation failed. Request: {JsonSerializer.Serialize(request)}");
            else
                _logger.LogInformation($"Order creation successfull, order id: {response.Id}");
        }

        async Task ProcessPaymentProcessed(PaymentProcessed payment)
        {
            if (payment == null || payment.OrderId == Guid.Empty)
                return;

            var status = string.Equals(payment.Status, "Completed", StringComparison.OrdinalIgnoreCase)
                ? "Paid"
                : "PaymentFailed";
            await _mediator.Send(new UpdateOrderStatus { OrderId = payment.OrderId, Status = status });
        }

        async Task ProcessFulfilmentUpdated(FulfilmentUpdated fulfilment)
        {
            if (fulfilment == null || fulfilment.OrderId == Guid.Empty)
                return;

            await _mediator.Send(new UpdateOrderStatus { OrderId = fulfilment.OrderId, Status = fulfilment.Status });
        }
    }
}
