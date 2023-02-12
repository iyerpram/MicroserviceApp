namespace MicroserviceApp.Logic.Abstractions.Providers
{
    public interface IDbProvider<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get<TIdentity>(TIdentity id);
        TEntity Add(TEntity entity);
        bool Update(TEntity entity);
        bool Delete<TIdentity>(TIdentity id, TEntity entity);
        IEnumerable<TResponse> RetrieveData<TRequest, TResponse>(TRequest request, IEnumerable<string> dataParams);
        bool ModifyData<TRequest>(TRequest request, IEnumerable<string> dataParams);
    }
}
