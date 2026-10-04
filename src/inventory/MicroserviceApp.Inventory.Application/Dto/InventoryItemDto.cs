namespace MicroserviceApp.Inventory.Application.Dto
{
    public class InventoryItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public int QuantityAvailable { get; set; }
    }
}
