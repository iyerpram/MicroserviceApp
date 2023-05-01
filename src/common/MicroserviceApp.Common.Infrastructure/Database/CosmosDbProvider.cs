using MicroserviceApp.Common.Abstractions.Database;
using Microsoft.Extensions.Configuration;

namespace MicroserviceApp.Common.Infrastructure.Database
{
    public class CosmosDbProvider<TEntity> : IExtendedDbProvider<TEntity> where TEntity : class
    {
        public IConfiguration Configuration { get; }
        public string _connectionString => Configuration?["Database:ConnectionString"];

        public CosmosDbProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<bool> CreateItemAsync(string container, TEntity item)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateItemAsync(string container, string id, TEntity item)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> GetItemAsync(string container, string id)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> DeleteItemAsync(string container, string id, TEntity item)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string container, string query)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExecuteCommandAsync(string container, string query)
        {
            throw new NotImplementedException();
        }
    }
}
