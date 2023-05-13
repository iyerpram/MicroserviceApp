using MicroserviceApp.Cart.Application;
using MicroserviceApp.Cart.Application.Mappings;
using MicroserviceApp.Common.Abstractions.Database;
using MicroserviceApp.Common.Abstractions.Messaging;
using MicroserviceApp.Common.Infrastructure;
using MicroserviceApp.Common.Infrastructure.Database;
using MicroserviceApp.Common.Infrastructure.Messaging;
using MicroserviceApp.Orders.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceApp.Cart.Infrastructure
{
    public static class Dependency
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.ConfigureAppServices(typeof(CartService));
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddSingleton<IMessagingProvider, AwsSnsMessagingProvider>();
            builder.Services.AddSingleton(typeof(IDbProvider<>), typeof(DynamoDbProvider<>));
            builder.Services.AddSingleton<ICartRepository, CartRepository>();
        }

        public static void ConfigureApi(this IApplicationBuilder app)
        {
            app.ConfigureApp();
        }
    }
}
