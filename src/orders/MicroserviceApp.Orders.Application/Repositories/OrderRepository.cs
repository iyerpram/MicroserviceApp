using AutoMapper;
using MicroserviceApp.Common.Abstractions.Database;
using MicroserviceApp.Common.Domain.Models;
using MicroserviceApp.Orders.Application.RequestHandlers;
using MicroserviceApp.Orders.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace MicroserviceApp.Orders.Application
{
    public interface IOrderRepository
    {
        Task<OrderDto> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<OrderDto>> GetOrdersAsync(string userId);
        Task<OrderDto> CreateOrderAsync(CreateOrder request);
    }

    public class OrderRepository : IOrderRepository
    {
        private IExtendedDbProvider<Order> _dbProvider { get; }
        public IConfiguration _configuration { get; }
        public IMapper _mapper { get; }

        public OrderRepository(IExtendedDbProvider<Order> dbProvider, IConfiguration configuration, IMapper mapper)
        {
            _dbProvider = dbProvider;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync(string userId)
        {
            return await _dbProvider.ExecuteQueryAsync<OrderDto>($"select * from orders where userId={userId}");
        }

        public async Task<OrderDto> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _dbProvider.GetItemAsync(orderId.ToString());
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrder request)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                Products = _mapper.Map<IEnumerable<Product>>(request.Products),
                User = _mapper.Map<User>(request.User)
            };
            var isAdded = await _dbProvider.CreateItemAsync(order);

            if (isAdded)
                return await Task.FromResult(_mapper.Map<OrderDto>(order));
            else
                return null;
        }
    }
}
