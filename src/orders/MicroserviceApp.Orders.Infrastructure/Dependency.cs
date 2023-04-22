using MicroserviceApp.Common.Infrastructure;
using MicroserviceApp.Common.Infrastructure.Database;
using MicroserviceApp.Common.Infrastructure.Messaging;
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
            builder.Services.AddSingleton<IMessagingProvider, AzureServiceBusMessagingProvider>();
            builder.Services.AddSingleton<IDbProvider, CosmosDbProvider>();
        }

        public static void ConfigureApi(this IApplicationBuilder app)
        {
            app.ConfigureApp();
        }
    }
}
