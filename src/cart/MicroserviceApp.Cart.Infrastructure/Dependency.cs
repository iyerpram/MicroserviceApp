using MicroserviceApp.Cart.Application;
using MicroserviceApp.Common.Abstractions.Database;
using MicroserviceApp.Common.Abstractions.Messaging;
using MicroserviceApp.Common.Infrastructure;
using MicroserviceApp.Common.Infrastructure.Database;
using MicroserviceApp.Common.Infrastructure.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceApp.Cart.Infrastructure
{
    public static class Dependency
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.ConfigureAppServices(typeof(CartService));
            builder.Services.AddSingleton<IMessagingProvider, AwsSnsMessagingProvider>();
            builder.Services.AddSingleton(typeof(IDbProvider<>), typeof(DynamoDbProvider<>));
        }

        public static void ConfigureApi(this IApplicationBuilder app)
        {
            app.ConfigureApp();
        }
    }
}
