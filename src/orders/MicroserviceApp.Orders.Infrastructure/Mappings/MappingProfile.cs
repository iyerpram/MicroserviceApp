using AutoMapper;
using MicroserviceApp.Common.Application;
using MicroserviceApp.Common.Domain.Models;
using MicroserviceApp.Orders.Application;
using MicroserviceApp.Orders.Domain.Models;

namespace MicroserviceApp.Orders.Infrastructure.Mappings
{
    internal class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<UserDto, User>();
            CreateMap<ProductDto, Product>();
            CreateMap<OrderDto, Order>();
        }
    }
}
