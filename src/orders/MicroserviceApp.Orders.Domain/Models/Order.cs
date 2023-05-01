using MicroserviceApp.Common.Domain.Models;

namespace MicroserviceApp.Orders.Domain.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public User? User { get; set; }
        public IEnumerable<Product>? Products { get; set; }
    }
}
