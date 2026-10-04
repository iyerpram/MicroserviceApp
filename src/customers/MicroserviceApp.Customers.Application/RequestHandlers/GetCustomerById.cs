using MediatR;
using MicroserviceApp.Customers.Application.Dto;

namespace MicroserviceApp.Customers.Application.RequestHandlers
{
    public class GetCustomerById : IRequest<CustomerDto>
    {
        public Guid CustomerId { get; set; }
    }

    public class GetCustomerByIdHandler : IRequestHandler<GetCustomerById, CustomerDto>
    {
        public ICustomerRepository _customerRepository { get; }

        public GetCustomerByIdHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerDto> Handle(GetCustomerById request, CancellationToken cancellationToken)
        {
            if (request.CustomerId == Guid.Empty)
                return null;

            return await _customerRepository.GetCustomerByIdAsync(request.CustomerId);
        }
    }
}
