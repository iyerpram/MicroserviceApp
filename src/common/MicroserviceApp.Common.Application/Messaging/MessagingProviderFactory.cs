using MicroserviceApp.Common.Application.Messaging.Enums;
using Microsoft.Extensions.Configuration;

namespace MicroserviceApp.Common.Application.Messaging
{
    public class MessagingProviderFactory : IMessagingProviderFactory
    {
        public IConfiguration Configuration { get; }

        public MessagingProviderFactory(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IMessagingProvider GetMessagingProvider(MessagingProviderType providerType, string configSection = "")
        {
            return providerType switch
            {
                MessagingProviderType.AzureServiceBus => new AzureServiceBusMessagingProvider(Configuration, configSection),
                MessagingProviderType.AWS_SNS => new AwsSnsMessagingProvider(Configuration, configSection),
                _ => new AzureServiceBusMessagingProvider(Configuration, configSection)
            };
        }
    }
}
