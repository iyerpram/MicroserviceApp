using MicroserviceApp.Common.Application;

namespace MicroserviceApp.Common.Application.Events
{
    public class InventoryReserved
    {
        public Guid ReservationId { get; set; }
        public Guid OrderId { get; set; }
        public UserDto? User { get; set; }
        public IEnumerable<ProductDto>? Products { get; set; }
        public bool Success { get; set; }
    }
}
