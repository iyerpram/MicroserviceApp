using MediatR;
using MicroserviceApp.Customers.Application.Dto;

namespace MicroserviceApp.Customers.Application.RequestHandlers
{
    public class GetCustomers : IRequest<IEnumerable<CustomerDto>>
    {
    }

    public class GetCustomersHandler : IRequestHandler<GetCustomers, IEnumerable<CustomerDto>>
    {
        public ICustomerRepository _customerRepository { get; }

        public GetCustomersHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<CustomerDto>> Handle(GetCustomers request, CancellationToken cancellationToken)
        {
            return await _customerRepository.GetCustomersAsync();
        }
    }
}
