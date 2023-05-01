using MicroserviceApp.Common.Application;

namespace MicroserviceApp.Orders.Application
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public UserDto? User { get; set; }
        public IEnumerable<ProductDto>? Products { get; set; }

    }
}
