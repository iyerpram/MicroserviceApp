namespace MicroserviceApp.Inventory.Domain.Models
{
    public class InventoryReservation
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public IEnumerable<Product>? Products { get; set; }
        public bool IsReserved { get; set; }
    }
}
