using MicroserviceApp.Common.Application;

namespace MicroserviceApp.Common.Application.Events
{
    public class FulfilmentUpdated
    {
        public Guid FulfilmentId { get; set; }
        public Guid OrderId { get; set; }
        public UserDto? User { get; set; }
        public string Status { get; set; } = "Created";
        public string? TrackingNumber { get; set; }
    }
}
