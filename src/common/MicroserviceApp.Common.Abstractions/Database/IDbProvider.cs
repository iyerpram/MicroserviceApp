namespace MicroserviceApp.Common.Abstractions.Database
{
    public interface IDbProvider
    {
        public Task<bool> CreateItemAsync<T>(string container, T item);
        public Task<bool> UpdateItemAsync<T>(string container, string id, T item);
        public Task<T> GetItemAsync<T>(string container, string id);        
        public Task<T> DeleteItemAsync<T>(string container, string id, T item);
    }
}
