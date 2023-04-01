using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace MicroserviceApp.Common.Application.Messaging
{
    public class AzureServiceBusMessagingProvider : IMessagingProvider
    {
        private IConfiguration Configuration { get; }
        public string ConfigSection { get; }

        private string _topic => Configuration[$"{ConfigSection}:ServiceBus:TopicName"];

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
            //logic to publish SNS message
            return await Task.FromResult(true);
        }

        public async Task<T> SubscribeMessageAsync<T>(string subject)
        {
            //logic to read message from SNS
            var messageBody = "";
            return await Task.FromResult(JsonSerializer.Deserialize<T>(messageBody));
        }
    }
}
