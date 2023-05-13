using MediatR;
using MicroserviceApp.Common.Application;
using MicroserviceApp.Orders.Application;

namespace MicroserviceApp.Cart.Application.RequestHandlers
{
    public class AddItemToCart : IRequest<bool>
    {
        public UserDto? User { get; set; }
        public ProductDto Product { get; set; }
    }

    public class AddItemToCartHandler : IRequestHandler<AddItemToCart, bool>
    {
        public ICartRepository _repository { get; }

        public AddItemToCartHandler(ICartRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(AddItemToCart request, CancellationToken cancellationToken)
        {
            if (request?.User == null || request?.Product == null)
                return false;

            return await _repository.AddItemToCartAsync(request);
        }
    }
}
