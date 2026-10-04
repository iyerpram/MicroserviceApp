using MediatR;
using MicroserviceApp.Payment.Application.Dto;

namespace MicroserviceApp.Payment.Application.RequestHandlers
{
    public class GetPaymentById : IRequest<PaymentDto>
    {
        public Guid PaymentId { get; set; }
    }

    public class GetPaymentByIdHandler : IRequestHandler<GetPaymentById, PaymentDto>
    {
        public IPaymentRepository _paymentRepository { get; }

        public GetPaymentByIdHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<PaymentDto> Handle(GetPaymentById request, CancellationToken cancellationToken)
        {
            if (request.PaymentId == Guid.Empty)
                return null;

            return await _paymentRepository.GetPaymentByIdAsync(request.PaymentId);
        }
    }
}
