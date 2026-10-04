using MediatR;
using MicroserviceApp.Common.Abstractions.Messaging;
using MicroserviceApp.Common.Application.Events;
using MicroserviceApp.Customers.Application.RequestHandlers;
using System.Text.Json;

namespace MicroserviceApp.Customers.Api.Observers
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
            _messagingProvider.SubscribeMessageAsync<CreateCustomer>(ProcessCreateCustomer);
            _messagingProvider.SubscribeMessageAsync<OrderCreated>(ProcessOrderCreated);
        }

        async Task ProcessCreateCustomer(CreateCustomer request)
        {
            if (request?.User == null)
                _logger.LogWarning($"Invalid customer creation request received. Request: {JsonSerializer.Serialize(request)}");

            var response = await _mediator.Send(request);
            if (response == null)
                _logger.LogWarning($"Customer creation failed. Request: {JsonSerializer.Serialize(request)}");
            else
                _logger.LogInformation($"Customer creation successfull, customer id: {response.Id}");
        }

        Task ProcessOrderCreated(OrderCreated order)
        {
            _logger.LogInformation($"Customer service observed order {order?.OrderId} for user {order?.User?.Id}");
            return Task.CompletedTask;
        }
    }
}
