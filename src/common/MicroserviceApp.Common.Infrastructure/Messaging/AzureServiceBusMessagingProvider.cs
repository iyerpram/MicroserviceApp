using MicroserviceApp.Common.Abstractions.Messaging;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace MicroserviceApp.Common.Infrastructure.Messaging
{
    public class AzureServiceBusMessagingProvider : IMessagingProvider
    {
        private IConfiguration Configuration { get; }
        public string ConfigSection { get; }

        private string _topicName => Configuration[$"{ConfigSection}:ServiceBus:TopicName"];
        private string _subscriptionName => Configuration[$"{ConfigSection}:ServiceBus:SubscriptionName"];
        private string _connectionString => Configuration[$"{ConfigSection}:ServiceBus:ConnectionString"];

        public AzureServiceBusMessagingProvider(IConfiguration configuration)
        {
            Configuration = configuration;
            ConfigSection = "Messaging";
        }
        public AzureServiceBusMessagingProvider(IConfiguration configuration, string configSection = "")
        {
            Configuration = configuration;
            ConfigSection = !string.IsNullOrWhiteSpace(configSection) ? $"Messaging:{configSection}" : "Messaging";
        }

        public async Task<bool> PublishMessageAsync<T>(string subject, T messageBody)
        {
            var message = JsonSerializer.Serialize(messageBody);
            //logic to publish Service Bus message
            return await Task.FromResult(true);
        }

        public async Task<T> ReadMessageAsync<T>()
        {
            //logic to read message from SNS
            var messageBody = "";
            return await Task.FromResult(JsonSerializer.Deserialize<T>(messageBody));
        }

        public async Task SubscribeMessageAsync<TResponse>(Func<TResponse, Task> handler)
        {
            //var client = new SubscriptionClient(_connectionString, _topicName, _subscriptionName, ReceiveMode.PeekLock, null);
            //client.RegisterMessageHandler(
            //    async (message, token) =>
            //    {
            //        var response = JsonSerializer.Deserialize<T>(message);
            //        handler.DynamicInvoke(response);
            //        await client.CompleteAsync(message.SystemProperties.LockToken);
            //    },
            //    async (exception) => {

            //    }
            //);

            //logic to read message from Service Bus
            Task.Factory.StartNew(() => {
                string message = "", token = "";
                var response = JsonSerializer.Deserialize<TResponse>(message);
                handler.Invoke(response);
            });
        }
    }
}
