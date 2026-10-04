using AutoMapper;
using MicroserviceApp.Common.Application;
using MicroserviceApp.Inventory.Application.Dto;
using MicroserviceApp.Inventory.Domain.Models;

namespace MicroserviceApp.Inventory.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<InventoryItemDto, InventoryItem>().ReverseMap();
            CreateMap<InventoryReservationDto, InventoryReservation>().ReverseMap();
        }
    }
}
