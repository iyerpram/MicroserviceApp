using AutoMapper;
using MicroserviceApp.Common.Application;
using MicroserviceApp.Common.Domain.Models;
using MicroserviceApp.Fulfilment.Application.Dto;

namespace MicroserviceApp.Fulfilment.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<FulfilmentDto, Domain.Models.Fulfilment>().ReverseMap();
        }
    }
}
