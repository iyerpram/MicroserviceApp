using AutoMapper;
using MicroserviceApp.Common.Abstractions.Database;
using MicroserviceApp.Common.Application;
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

        private string _container => _configuration?["Database:Container"] ?? string.Empty;

        public OrderRepository(IExtendedDbProvider<Order> dbProvider, IConfiguration configuration, IMapper mapper)
        {
            _dbProvider = dbProvider;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync(string userId)
        {
            //return await _dbProvider.ExecuteQueryAsync<OrderDto>(_container, $"select * from orders where userId={userId}");
            return await Task.FromResult(new List<OrderDto> {
                new OrderDto {
                    Id = Guid.NewGuid(),
                    Products = new List<ProductDto> {
                        new ProductDto{
                            Id = Guid.NewGuid(),
                            Description = "test 1",
                            Name = "test 1",
                            Price = 10
                        },
                        new ProductDto{
                            Id = Guid.NewGuid(),
                            Description = "test 2",
                            Name = "test 2",
                            Price = 20
                        }
                    },
                    User = new UserDto{
                        Id = 1,
                        Name = "Test User",
                        Email = "test@mail.com"
                    }
                }
            });
        }

        public async Task<OrderDto> GetOrderByIdAsync(Guid orderId)
        {
            //var order = await _dbProvider.GetItemAsync(_container, orderId.ToString());
            //return _mapper.Map<OrderDto>(order);

            return await Task.FromResult(new OrderDto
            {
                Id = Guid.NewGuid(),
                Products = new List<ProductDto> {
                    new ProductDto {
                        Id = Guid.NewGuid(),
                        Description = "test 1",
                        Name = "test 1",
                        Price = 10
                    },
                    new ProductDto {
                        Id = Guid.NewGuid(),
                        Description = "test 2",
                        Name = "test 2",
                        Price = 20
                    }
                },
                User = new UserDto
                {
                    Id = 1,
                    Name = "Test User",
                    Email = "test@mail.com"
                }
            });
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrder request)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                Products = _mapper.Map<IEnumerable<Product>>(request.Products),
                User = _mapper.Map<User>(request.User)
            };
            //var isAdded = await _dbProvider.CreateItemAsync(_container, order);

            return await Task.FromResult(_mapper.Map<OrderDto>(order));
        }
    }
}
