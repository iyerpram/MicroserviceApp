using MicroserviceApp.Common.Application;

namespace MicroserviceApp.Customers.Application.Dto
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public UserDto? User { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
