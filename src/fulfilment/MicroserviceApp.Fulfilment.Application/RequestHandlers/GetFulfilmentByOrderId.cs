using MediatR;
using MicroserviceApp.Fulfilment.Application.Dto;

namespace MicroserviceApp.Fulfilment.Application.RequestHandlers
{
    public class GetFulfilmentByOrderId : IRequest<FulfilmentDto>
    {
        public Guid OrderId { get; set; }
    }

    public class GetFulfilmentByOrderIdHandler : IRequestHandler<GetFulfilmentByOrderId, FulfilmentDto>
    {
        public IFulfilmentRepository _fulfilmentRepository { get; }

        public GetFulfilmentByOrderIdHandler(IFulfilmentRepository fulfilmentRepository)
        {
            _fulfilmentRepository = fulfilmentRepository;
        }

        public async Task<FulfilmentDto> Handle(GetFulfilmentByOrderId request, CancellationToken cancellationToken)
        {
            if (request.OrderId == Guid.Empty)
                return null;

            return await _fulfilmentRepository.GetFulfilmentByOrderIdAsync(request.OrderId);
        }
    }
}
