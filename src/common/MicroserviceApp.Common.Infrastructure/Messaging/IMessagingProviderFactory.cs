using MicroserviceApp.Common.Abstractions.Messaging;
using MicroserviceApp.Common.Application.Messaging.Enums;

namespace MicroserviceApp.Common.Infrastructure.Messaging
{
    public interface IMessagingProviderFactory
    {
        IMessagingProvider GetMessagingProvider(MessagingProviderType providerType, string configSection = "");
    }
}