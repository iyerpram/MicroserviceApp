using MicroserviceApp.Common.Application;

namespace MicroserviceApp.Fulfilment.Application.Dto
{
    public class FulfilmentDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public UserDto? User { get; set; }
        public string Status { get; set; } = "Created";
        public string? TrackingNumber { get; set; }
    }
}
