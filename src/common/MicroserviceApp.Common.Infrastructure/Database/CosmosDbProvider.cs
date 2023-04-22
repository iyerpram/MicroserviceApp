using Microsoft.Extensions.Configuration;

namespace MicroserviceApp.Common.Infrastructure.Database
{
    public class CosmosDbProvider : IExtendedDbProvider
    {
        public IConfiguration Configuration { get; }
        public string _connectionString => Configuration["Database:ConnectionString"];

        public CosmosDbProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<bool> CreateItemAsync<T>(string container, T item)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateItemAsync<T>(string container, string id, T item)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetItemAsync<T>(string container, string id)
        {
            throw new NotImplementedException();
        }

        public async Task<T> DeleteItemAsync<T>(string container, string id, T item)
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
