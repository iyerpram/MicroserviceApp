using MediatR;
using MicroserviceApp.Common.Abstractions.Messaging;
using MicroserviceApp.Orders.Application;
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
        public IMessagingProviderFactory _messageProviderFactory { get; }

        public MessageObserver(IMessagingProvider messagingProvider, IMediator mediator
            , ILogger<MessageObserver> logger, IMessagingProviderFactory messageProviderFactory) 
        {
            _messagingProvider = messagingProvider;
            _mediator = mediator;
            _logger = logger;
            _messageProviderFactory = messageProviderFactory;
        }        

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ReadMessage, null, 0, 10000);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //New Timer does not have a stop. 
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        void ReadMessage(object state)
        {
            _messagingProvider.SubscribeMessageAsync<CreateOrder>(ProcessMessage);
        }

        async Task ProcessMessage(CreateOrder request)
        {
            if (request?.User == null || (!request?.Products?.Any() ?? true))
                _logger.LogWarning($"Invalid order creation request received. Request: {JsonSerializer.Serialize(request)}");

            var response = await _mediator.Send(request);
            if (response == null)
                _logger.LogWarning($"Order creation failed. Request: {JsonSerializer.Serialize(request)}");

            _logger.LogInformation($"Order creation successfull, order id: {response.Id}");

            var cartMessageProvider = _messageProviderFactory.GetMessagingProvider(MessagingProviderType.AWS_SNS, "Cart");
            var isPublished = await cartMessageProvider.PublishMessageAsync("Order Creation Successfull", response);
            if(isPublished)
                _logger.LogInformation($"Order creation message posted successfully, order id: {response.Id}");
            else
                _logger.LogInformation($"Order creation message failed, order id: {response.Id}");
        }
    }
}
