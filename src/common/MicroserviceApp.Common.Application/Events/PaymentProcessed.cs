using MicroserviceApp.Common.Application;

namespace MicroserviceApp.Common.Application.Events
{
    public class PaymentProcessed
    {
        public Guid PaymentId { get; set; }
        public Guid OrderId { get; set; }
        public UserDto? User { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Pending";
        public IEnumerable<ProductDto>? Products { get; set; }
    }
}
