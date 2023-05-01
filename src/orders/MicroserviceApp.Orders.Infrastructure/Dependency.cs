using AutoMapper;
using MicroserviceApp.Common.Abstractions.Database;
using MicroserviceApp.Common.Abstractions.Messaging;
using MicroserviceApp.Common.Infrastructure;
using MicroserviceApp.Common.Infrastructure.Database;
using MicroserviceApp.Common.Infrastructure.Messaging;
using MicroserviceApp.Orders.Application;
using MicroserviceApp.Orders.Infrastructure.Mappings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceApp.Orders.Infrastructure
{
    public static class Dependency
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.ConfigureAppServices(typeof(OrderRepository));
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddSingleton<IMessagingProvider, AzureServiceBusMessagingProvider>();
            builder.Services.AddSingleton(typeof(IDbProvider<>), typeof(CosmosDbProvider<>));
            builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
        }

        public static void ConfigureApi(this IApplicationBuilder app)
        {
            app.ConfigureApp();
        }
    }
}
