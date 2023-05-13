using MicroserviceApp.Common.Domain.Models;

namespace MicroserviceApp.Cart.Domain.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public List<Product> Products { get; set; }
    }
}
