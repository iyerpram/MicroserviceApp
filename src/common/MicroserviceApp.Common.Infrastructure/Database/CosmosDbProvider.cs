using MicroserviceApp.Common.Abstractions.Database;
using Microsoft.Extensions.Configuration;

namespace MicroserviceApp.Common.Infrastructure.Database
{
    public class CosmosDbProvider<TEntity> : IExtendedDbProvider<TEntity> where TEntity : class
    {
        public IConfiguration Configuration { get; }
        public string _connectionString => Configuration?["Database:ConnectionString"];
        public string _container => Configuration?["Database:Container"];

        public CosmosDbProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<bool> CreateItemAsync(TEntity item)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateItemAsync(string id, TEntity item)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> DeleteItemAsync(string id, TEntity item)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExecuteCommandAsync(string query)
        {
            throw new NotImplementedException();
        }
    }
}
