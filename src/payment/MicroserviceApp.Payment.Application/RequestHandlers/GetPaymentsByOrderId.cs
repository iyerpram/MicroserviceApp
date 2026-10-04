using MediatR;
using MicroserviceApp.Payment.Application.Dto;

namespace MicroserviceApp.Payment.Application.RequestHandlers
{
    public class GetPaymentsByOrderId : IRequest<IEnumerable<PaymentDto>>
    {
        public Guid OrderId { get; set; }
    }

    public class GetPaymentsByOrderIdHandler : IRequestHandler<GetPaymentsByOrderId, IEnumerable<PaymentDto>>
    {
        public IPaymentRepository _paymentRepository { get; }

        public GetPaymentsByOrderIdHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<PaymentDto>> Handle(GetPaymentsByOrderId request, CancellationToken cancellationToken)
        {
            if (request.OrderId == Guid.Empty)
                return null;

            return await _paymentRepository.GetPaymentsByOrderIdAsync(request.OrderId);
        }
    }
}
