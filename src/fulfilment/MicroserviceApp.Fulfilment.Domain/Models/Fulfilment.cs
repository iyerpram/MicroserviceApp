using MicroserviceApp.Common.Domain.Models;

namespace MicroserviceApp.Fulfilment.Domain.Models
{
    public class Fulfilment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public User? User { get; set; }
        public string Status { get; set; } = "Created";
        public string? TrackingNumber { get; set; }
    }
}
