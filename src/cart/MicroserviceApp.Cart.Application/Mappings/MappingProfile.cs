using AutoMapper;
using MicroserviceApp.Cart.Domain.Models;
using MicroserviceApp.Common.Application;
using MicroserviceApp.Common.Domain.Models;

namespace MicroserviceApp.Cart.Application.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<UserDto, User>();
            CreateMap<ProductDto, Product>();
        }
    }
}
