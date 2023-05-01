using MediatR;
using MicroserviceApp.Common.Application;

namespace MicroserviceApp.Orders.Application.RequestHandlers
{
    public class CreateOrder: IRequest<OrderDto>
    {
        public UserDto? User { get; set; }
        public IEnumerable<ProductDto>? Products { get; set; }
    }

    public class CreateOrderHandler : IRequestHandler<CreateOrder, OrderDto>
    {
        public IOrderRepository _orderRepository { get; }

        public CreateOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }        

        public async Task<OrderDto> Handle(CreateOrder request, CancellationToken cancellationToken)
        {
            if (request?.User == null)
                return null;

            return await _orderRepository.CreateOrderAsync(request);
        }
    }
}
