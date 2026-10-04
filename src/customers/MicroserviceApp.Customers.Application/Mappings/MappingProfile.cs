using AutoMapper;
using MicroserviceApp.Common.Application;
using MicroserviceApp.Common.Domain.Models;
using MicroserviceApp.Customers.Application.Dto;
using MicroserviceApp.Customers.Domain.Models;

namespace MicroserviceApp.Customers.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<CustomerDto, Customer>().ReverseMap();
        }
    }
}
