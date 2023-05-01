namespace MicroserviceApp.Common.Abstractions.Messaging
{
    public interface IMessagingProvider
    {
        Task<bool> PublishMessageAsync<T>(string subject, T messageBody);
        Task<T> ReadMessageAsync<T>();
        Task SubscribeMessageAsync<T>(Delegate handler);
    }
}
