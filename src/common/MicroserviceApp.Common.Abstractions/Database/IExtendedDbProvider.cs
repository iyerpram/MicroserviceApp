namespace MicroserviceApp.Common.Abstractions.Database
{
    public interface IExtendedDbProvider<TEntity>: IDbProvider<TEntity> where TEntity : class
    {
        public Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query);
        public Task<bool> ExecuteCommandAsync(string query);
    }
}
