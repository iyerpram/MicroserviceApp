namespace MicroserviceApp.Inventory.Domain.Models
{
    public class InventoryItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public int QuantityAvailable { get; set; }
    }
}
