using MicroserviceApp.Logic.Application.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceApp.Logic.Infrastructure
{
    public static class ServiceDependency
    {
        public static void AddApplicationServices(this IServiceCollection services)
        { 
            
        }

        public static void AddDataServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));
        }
    }
}
