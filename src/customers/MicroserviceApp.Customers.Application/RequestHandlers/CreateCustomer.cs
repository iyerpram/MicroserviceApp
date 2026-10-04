using MediatR;
using MicroserviceApp.Common.Application;
using MicroserviceApp.Customers.Application.Dto;

namespace MicroserviceApp.Customers.Application.RequestHandlers
{
    public class CreateCustomer : IRequest<CustomerDto>
    {
        public UserDto? User { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }

    public class CreateCustomerHandler : IRequestHandler<CreateCustomer, CustomerDto>
    {
        public ICustomerRepository _customerRepository { get; }

        public CreateCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerDto> Handle(CreateCustomer request, CancellationToken cancellationToken)
        {
            if (request?.User == null)
                return null;

            return await _customerRepository.CreateCustomerAsync(request);
        }
    }
}
