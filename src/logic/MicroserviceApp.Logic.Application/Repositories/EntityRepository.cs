namespace MicroserviceApp.Logic.Application.Repositories
{
    public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {        
        public IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public TEntity Get<TIdentity>(TIdentity id)
        {
            throw new NotImplementedException();
        }
        public TEntity Add(TEntity entity)
        {
            throw new NotImplementedException();
        }
        public bool Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete<TIdentity>(TIdentity id, TEntity entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TResponse> RetrieveData<TRequest, TResponse>(TRequest request, IEnumerable<string> dataParams)
        {
            throw new NotImplementedException();
        }

        public bool ModifyData<TRequest>(TRequest request, IEnumerable<string> dataParams)
        {
            throw new NotImplementedException();
        }
    }
}
