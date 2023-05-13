using MediatR;
using MicroserviceApp.Cart.Application.Dto;
using MicroserviceApp.Common.Application;
using MicroserviceApp.Orders.Application;

namespace MicroserviceApp.Cart.Application.RequestHandlers
{
    public class GetCartItems : IRequest<CartItemsDto>
    {
        public UserDto? User { get; set; }
    }

    public class GetCartItemsHandler : IRequestHandler<GetCartItems, CartItemsDto>
    {
        public ICartRepository _repository { get; }

        public GetCartItemsHandler(ICartRepository repository)
        {
            _repository = repository;
        }

        public async Task<CartItemsDto> Handle(GetCartItems request, CancellationToken cancellationToken)
        {
            if (request?.User == null)
                return null;

            return await _repository.GetCartItemsAsync(request.User.Id.ToString());
        }
    }
}
