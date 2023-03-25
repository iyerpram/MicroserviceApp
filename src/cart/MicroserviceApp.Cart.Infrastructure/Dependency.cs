using MicroserviceApp.Cart.Application;
using MicroserviceApp.Common.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceApp.Cart.Infrastructure
{
    public static class Dependency
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.ConfigureAppServices(typeof(CartService));
        }

        public static void ConfigureApi(this IApplicationBuilder app)
        {
            app.ConfigureApp();
        }
    }
}
