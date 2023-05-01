namespace MicroserviceApp.Common.Abstractions.Database
{
    public interface IDbProvider<TEntity> where TEntity : class
    {
        public Task<bool> CreateItemAsync(string container, TEntity item);
        public Task<bool> UpdateItemAsync(string container, string id, TEntity item);
        public Task<TEntity> GetItemAsync(string container, string id);        
        public Task<TEntity> DeleteItemAsync(string container, string id, TEntity item);
    }
}
