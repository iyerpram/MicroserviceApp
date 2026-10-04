using MicroserviceApp.Common.Domain.Models;

namespace MicroserviceApp.Customers.Domain.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public User? User { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
