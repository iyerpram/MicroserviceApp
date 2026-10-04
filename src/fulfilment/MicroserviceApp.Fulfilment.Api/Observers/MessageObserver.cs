using MediatR;
using MicroserviceApp.Common.Abstractions.Messaging;
using MicroserviceApp.Common.Application.Events;
using MicroserviceApp.Fulfilment.Application.RequestHandlers;
using System.Text.Json;

namespace MicroserviceApp.Fulfilment.Api.Observers
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
            _messagingProvider.SubscribeMessageAsync<InventoryReserved>(ProcessInventoryReserved);
        }

        async Task ProcessInventoryReserved(InventoryReserved reservation)
        {
            if (reservation == null || !reservation.Success || reservation.OrderId == Guid.Empty || reservation.User == null)
            {
                _logger.LogWarning($"Fulfilment skipped. Request: {JsonSerializer.Serialize(reservation)}");
                return;
            }

            var response = await _mediator.Send(new CreateFulfilment
            {
                OrderId = reservation.OrderId,
                User = reservation.User
            });

            if (response == null)
                _logger.LogWarning($"Fulfilment creation failed for order {reservation.OrderId}");
            else
                _logger.LogInformation($"Fulfilment creation successfull, fulfilment id: {response.Id}");
        }
    }
}
