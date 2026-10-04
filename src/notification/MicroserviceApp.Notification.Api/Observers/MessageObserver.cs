using MediatR;
using MicroserviceApp.Common.Abstractions.Messaging;
using MicroserviceApp.Common.Application.Events;
using MicroserviceApp.Notification.Application.RequestHandlers;
using System.Text.Json;

namespace MicroserviceApp.Notification.Api.Observers
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
            _messagingProvider.SubscribeMessageAsync<NotificationRequested>(ProcessNotificationRequested);
        }

        async Task ProcessNotificationRequested(NotificationRequested request)
        {
            if (request == null)
            {
                _logger.LogWarning($"Invalid notification request received. Request: {JsonSerializer.Serialize(request)}");
                return;
            }

            var response = await _mediator.Send(new SendNotification
            {
                UserId = request.UserId,
                Email = request.Email,
                Subject = request.Subject,
                Body = request.Body,
                EventType = request.EventType,
                RelatedEntityId = request.RelatedEntityId
            });

            if (response == null)
                _logger.LogWarning($"Notification send failed. Request: {JsonSerializer.Serialize(request)}");
            else
                _logger.LogInformation($"Notification sent, notification id: {response.Id}");
        }
    }
}
