using MicroserviceApp.Common.Application.Messaging.Enums;
using Microsoft.Extensions.Configuration;

namespace MicroserviceApp.Common.Application.Messaging
{
    public interface IMessagingProviderFactory
    {
        IMessagingProvider GetMessagingProvider(MessagingProviderType providerType, string configSection = "");
    }
}