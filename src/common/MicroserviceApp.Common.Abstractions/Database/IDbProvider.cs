namespace MicroserviceApp.Common.Abstractions.Database
{
    public interface IDbProvider<TEntity> where TEntity : class
    {
        public Task<bool> CreateItemAsync(TEntity item);
        public Task<bool> UpdateItemAsync(string id, TEntity item);
        public Task<TEntity> GetItemAsync(string id);        
        public Task<TEntity> DeleteItemAsync(string id, TEntity item);
    }
}
