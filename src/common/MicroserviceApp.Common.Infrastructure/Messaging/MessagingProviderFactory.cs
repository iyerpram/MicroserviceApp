using MicroserviceApp.Common.Abstractions.Messaging;
using Microsoft.Extensions.Configuration;

namespace MicroserviceApp.Common.Infrastructure.Messaging
{
    public class MessagingProviderFactory : IMessagingProviderFactory
    {
        public IConfiguration Configuration { get; }

        public MessagingProviderFactory(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IMessagingProvider GetMessagingProvider(MessagingProviderType providerType, string appName = "")
        {
            return providerType switch
            {
                MessagingProviderType.AzureServiceBus => new AzureServiceBusMessagingProvider(Configuration, appName),
                MessagingProviderType.AWS_SNS => new AwsSnsMessagingProvider(Configuration, appName),
                _ => new AzureServiceBusMessagingProvider(Configuration, appName)
            };
        }
    }
}
