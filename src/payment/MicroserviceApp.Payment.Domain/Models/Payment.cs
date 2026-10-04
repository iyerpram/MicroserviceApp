using MicroserviceApp.Common.Domain.Models;

namespace MicroserviceApp.Payment.Domain.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public User? User { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
