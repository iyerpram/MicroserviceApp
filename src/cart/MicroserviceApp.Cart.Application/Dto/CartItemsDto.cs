using MicroserviceApp.Common.Application;

namespace MicroserviceApp.Cart.Application.Dto
{
    public class CartItemsDto
    {
        public UserDto User { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
