using MediatR;

namespace MicroserviceApp.Orders.Application.RequestHandlers
{
    public class GetOrderById : IRequest<OrderDto>
    {
        public Guid OrderId { get; set; }
    }

    public class GetOrderByIdHandler : IRequestHandler<GetOrderById, OrderDto>
    {
        public IOrderRepository _orderRepository { get; }

        public GetOrderByIdHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDto> Handle(GetOrderById request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request?.OrderId.ToString() ?? string.Empty))
                return null;

            return await _orderRepository.GetOrderByIdAsync(request.OrderId);
        }
    }
}
