using AutoMapper;
using MicroserviceApp.Common.Abstractions.Database;
using MicroserviceApp.Common.Domain.Models;
using MicroserviceApp.Customers.Application.Dto;
using MicroserviceApp.Customers.Application.RequestHandlers;
using MicroserviceApp.Customers.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace MicroserviceApp.Customers.Application
{
    public interface ICustomerRepository
    {
        Task<CustomerDto> GetCustomerByIdAsync(Guid customerId);
        Task<IEnumerable<CustomerDto>> GetCustomersAsync();
        Task<CustomerDto> CreateCustomerAsync(CreateCustomer request);
    }

    public class CustomerRepository : ICustomerRepository
    {
        private IExtendedDbProvider<Customer> _dbProvider { get; }
        public IConfiguration _configuration { get; }
        public IMapper _mapper { get; }

        public CustomerRepository(IExtendedDbProvider<Customer> dbProvider, IConfiguration configuration, IMapper mapper)
        {
            _dbProvider = dbProvider;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(Guid customerId)
        {
            var customer = await _dbProvider.GetItemAsync(customerId.ToString());
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<IEnumerable<CustomerDto>> GetCustomersAsync()
        {
            return await _dbProvider.ExecuteQueryAsync<CustomerDto>("select * from customers");
        }

        public async Task<CustomerDto> CreateCustomerAsync(CreateCustomer request)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                User = _mapper.Map<User>(request.User),
                Phone = request.Phone,
                Address = request.Address
            };

            var isAdded = await _dbProvider.CreateItemAsync(customer);
            return isAdded ? _mapper.Map<CustomerDto>(customer) : null;
        }
    }
}
