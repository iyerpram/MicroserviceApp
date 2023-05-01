namespace MicroserviceApp.Common.Abstractions.Messaging
{
    public interface IMessagingProviderFactory
    {
        IMessagingProvider GetMessagingProvider(MessagingProviderType providerType, string appName = "");
    }
}