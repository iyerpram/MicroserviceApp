namespace MicroserviceApp.Orders.Application.Dto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public UserDto User { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }

    }
}
