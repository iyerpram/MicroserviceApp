using MicroserviceApp.Common.Abstractions.Messaging;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace MicroserviceApp.Common.Infrastructure.Messaging
{
    public class AwsSnsMessagingProvider : IMessagingProvider
    {
        private IConfiguration Configuration { get; }
        public string ConfigSection { get; }

        private string _topic => Configuration[$"{ConfigSection}:SNS:TopicName"];

        public AwsSnsMessagingProvider(IConfiguration configuration)
        {
            Configuration = configuration;
            ConfigSection = "Messaging";
        }
        public AwsSnsMessagingProvider(IConfiguration configuration, string appName = "") {
            Configuration = configuration;
            ConfigSection = !string.IsNullOrWhiteSpace(appName) ? $"Messaging:{appName}" : "Messaging";
        }
        
        public async Task<bool> PublishMessageAsync<T>(string subject, T messageBody)
        {
            var message = JsonSerializer.Serialize(messageBody);
            //logic to publish SNS message
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
            //logic to read message from Service Bus
            Task.Factory.StartNew(() => {
                string message = "", token = "";
                var response = JsonSerializer.Deserialize<TResponse>(message);
                handler.Invoke(response);
            });
        }
    }
}
