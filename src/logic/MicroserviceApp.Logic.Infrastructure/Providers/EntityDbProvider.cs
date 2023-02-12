using MicroserviceApp.Logic.Abstractions.Providers;
using MicroserviceApp.Logic.Domain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MicroserviceApp.Logic.Infrastructure.Providers
{
    public class EntityDbProvider<TEntity> : IDbProvider<TEntity> where TEntity : class
    {
        public DbContext DbContext { get; }
        public IConfiguration Configuration { get; }

        public EntityDbProvider(DbContext dbContext, IConfiguration configuration)
        {
            DbContext = dbContext;
            Configuration = configuration;
        }

        public async Task<bool> Add(TEntity entity)
        {
            DbContext.Add(entity);
            var result = await DbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> Delete<TIdentity>(TIdentity id, TEntity entity)
        {
            var entry = DbContext.Find<TEntity>(id);
            if (entry != null)
            {
                DbContext.Remove(entity);
                var result = await DbContext.SaveChangesAsync();
                return result > 0;
            }
            return false;
        }

        public TEntity Get<TIdentity>(TIdentity id)
        {
            var entry = DbContext.Find<TEntity>(id);
            if (entry != null)
                return entry;
            return null;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DbContext.Find<TEntity>();
        }

        public async Task<bool> ModifyData(string procName, IEnumerable<DataParameter> dataParams)
        {
            try
            {
                using (var con = new SqlConnection(Configuration["SqlConnectionString"]))
                {
                    con.Open();
                    var command = new SqlCommand(procName, con);
                    var dbParams = dataParams.Select(x =>
                    {
                        var param = new SqlParameter(x.Key, x.DbType);
                        param.Value = x.Value;
                        return param;
                    });
                    command.Parameters.Add(dbParams);
                    var result = await command.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<TResponse> RetrieveData<TResponse>(string procName, IEnumerable<DataParameter> dataParams) where TResponse : new()
        {
            try
            {
                using (var con = new SqlConnection(Configuration["SqlConnectionString"]))
                {
                    con.Open();
                    var command = new SqlCommand(procName, con);
                    var dbParams = dataParams.Select(x =>
                    {
                        var param = new SqlParameter(x.Key, x.DbType);
                        param.Value = x.Value;
                        return param;
                    });
                    command.Parameters.Add(dbParams);
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var ds = new DataSet();
                        adapter.Fill(ds);
                        var result = new TResponse();
                        var type = typeof(TResponse);
                        type.ins
                        type.GetMembers().Select(x => type.GetMember(x.Name).SetValue());
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public bool Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
