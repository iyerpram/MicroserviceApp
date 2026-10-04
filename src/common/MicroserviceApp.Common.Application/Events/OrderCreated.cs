using MicroserviceApp.Common.Application;

namespace MicroserviceApp.Common.Application.Events
{
    public class OrderCreated
    {
        public Guid OrderId { get; set; }
        public UserDto? User { get; set; }
        public IEnumerable<ProductDto>? Products { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
