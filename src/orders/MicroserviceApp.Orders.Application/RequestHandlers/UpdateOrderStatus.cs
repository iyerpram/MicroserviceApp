using MediatR;

namespace MicroserviceApp.Orders.Application.RequestHandlers
{
    public class UpdateOrderStatus : IRequest<OrderDto>
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatus, OrderDto>
    {
        public IOrderRepository _orderRepository { get; }

        public UpdateOrderStatusHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDto> Handle(UpdateOrderStatus request, CancellationToken cancellationToken)
        {
            if (request.OrderId == Guid.Empty || string.IsNullOrWhiteSpace(request.Status))
                return null;

            return await _orderRepository.UpdateOrderStatusAsync(request.OrderId, request.Status);
        }
    }
}
