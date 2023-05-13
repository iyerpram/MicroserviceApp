using AutoMapper;
using MicroserviceApp.Common.Application;
using MicroserviceApp.Common.Domain.Models;
using MicroserviceApp.Orders.Domain.Models;

namespace MicroserviceApp.Orders.Application.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<UserDto, User>();
            CreateMap<ProductDto, Product>();
            CreateMap<OrderDto, Order>();
        }
    }
}
