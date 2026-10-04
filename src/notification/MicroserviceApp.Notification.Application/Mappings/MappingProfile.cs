using AutoMapper;
using MicroserviceApp.Notification.Application.Dto;

namespace MicroserviceApp.Notification.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NotificationDto, Domain.Models.Notification>().ReverseMap();
        }
    }
}
