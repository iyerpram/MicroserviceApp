using MicroserviceApp.Common.Application;

namespace MicroserviceApp.Cart.Application.Dto
{
    public class CartItemDto
    {
        public int UserId { get; set; } 
        public ProductDto Product { get; set; }
    }
}
