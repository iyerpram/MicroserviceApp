using MicroserviceApp.Common.Application;

namespace MicroserviceApp.Inventory.Application.Dto
{
    public class InventoryReservationDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public IEnumerable<ProductDto>? Products { get; set; }
        public bool IsReserved { get; set; }
    }
}
