using MicroserviceApp.Common.Infrastructure;
using MicroserviceApp.Orders.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceApp.Orders.Infrastructure
{
    public static class Dependency
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.ConfigureAppServices(typeof(OrderService));
        }

        public static void ConfigureApi(this IApplicationBuilder app)
        {
            app.ConfigureApp();
        }
    }
}
