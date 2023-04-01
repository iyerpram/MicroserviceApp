namespace MicroserviceApp.Common.Application.Database
{
    public interface IDbProvider
    {
        public Task<bool> CreateItemAsync<T>(string container, T item);
        public Task<bool> UpdateItemAsync<T>(string container, string id, T item);
        public Task<T> GetItemAsync<T>(string container, string id);
        public Task<T> QueryAsync<T>(string container, string query);
        public Task<T> DeleteItemAsync<T>(string container, string id, T item);
    }
}
