using Microsoft.Extensions.Configuration;

namespace MicroserviceApp.Common.Application.Database
{
    public class CosmosDbProvider : IDbProvider
    {
        public IConfiguration Configuration { get; }
        public string _connectionString => Configuration["Database:ConnectionString"];

        public CosmosDbProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Task<bool> CreateItemAsync<T>(string container, T item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync<T>(string container, string id, T item)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetItemAsync<T>(string container, string id)
        {
            throw new NotImplementedException();
        }

        public Task<T> QueryAsync<T>(string container, string query)
        {
            throw new NotImplementedException();
        }

        public Task<T> DeleteItemAsync<T>(string container, string id, T item)
        {
            throw new NotImplementedException();
        }
    }
}
