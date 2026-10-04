using MicroserviceApp.Common.Application;

namespace MicroserviceApp.Payment.Application.Dto
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public UserDto? User { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
