using MicroserviceApp.Common.Infrastructure;
using MicroserviceApp.Customers.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceApp.Customers.Infrastructure
{
    public static class Dependency
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.ConfigureAppServices(typeof(CustomerService));
        }

        public static void ConfigureApi(this IApplicationBuilder app)
        {
            app.ConfigureApp();
        }
    }
}
