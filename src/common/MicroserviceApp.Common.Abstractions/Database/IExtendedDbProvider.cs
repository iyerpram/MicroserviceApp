namespace MicroserviceApp.Common.Abstractions.Database
{
    public interface IExtendedDbProvider: IDbProvider
    {
        public Task<IEnumerable<T>> ExecuteQueryAsync<T>(string container, string query);
        public Task<bool> ExecuteCommandAsync(string container, string query);
    }
}
