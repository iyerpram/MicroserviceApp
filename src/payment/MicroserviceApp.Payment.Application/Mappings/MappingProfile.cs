using AutoMapper;
using MicroserviceApp.Common.Application;
using MicroserviceApp.Common.Domain.Models;
using MicroserviceApp.Payment.Application.Dto;

namespace MicroserviceApp.Payment.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<PaymentDto, Domain.Models.Payment>().ReverseMap();
        }
    }
}
