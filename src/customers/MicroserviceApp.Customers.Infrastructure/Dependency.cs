using MicroserviceApp.Common.Abstractions.Database;
using MicroserviceApp.Common.Abstractions.Messaging;
using MicroserviceApp.Common.Infrastructure;
using MicroserviceApp.Common.Infrastructure.Database;
using MicroserviceApp.Common.Infrastructure.Messaging;
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
            builder.Services.AddSingleton<IMessagingProvider, AzureServiceBusMessagingProvider>();
            builder.Services.AddSingleton(typeof(IDbProvider<>), typeof(CosmosDbProvider<>));
        }

        public static void ConfigureApi(this IApplicationBuilder app)
        {
            app.ConfigureApp();
        }
    }
}
