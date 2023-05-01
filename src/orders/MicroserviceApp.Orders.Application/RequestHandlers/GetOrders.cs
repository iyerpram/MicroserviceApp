using MediatR;

namespace MicroserviceApp.Orders.Application.RequestHandlers
{
    public class GetOrders : IRequest<IEnumerable<OrderDto>>
    {
        public string? UserId { get; set; }
    }

    public class GetOrdersHandler : IRequestHandler<GetOrders, IEnumerable<OrderDto>>
    {
        public IOrderRepository _orderRepository { get; }

        public GetOrdersHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }        

        public async Task<IEnumerable<OrderDto>> Handle(GetOrders request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request?.UserId.ToString() ?? string.Empty))
                return null;

            return await _orderRepository.GetOrdersAsync(request.UserId);
        }
    }
}
