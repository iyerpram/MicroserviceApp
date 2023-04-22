using MediatR;
using MicroserviceApp.Common.Abstractions.Database;
using MicroserviceApp.Orders.Application.Dto;
using Microsoft.Extensions.Configuration;

namespace MicroserviceApp.Orders.Application.RequestHandlers
{
    public class GetOrders : IRequest<IEnumerable<OrderDto>>
    {
        public string UserId { get; set; }
    }

    public class GetOrdersHandler : IRequestHandler<GetOrders, IEnumerable<OrderDto>>
    {
        private IExtendedDbProvider _dbProvider { get; }
        public IConfiguration _configuration { get; }

        private string _container => _configuration?["Database:Container"] ?? string.Empty;
        public GetOrdersHandler(IExtendedDbProvider dbProvider, IConfiguration configuration)
        {
            _dbProvider = dbProvider;
            _configuration = configuration;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetOrders request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request?.UserId.ToString() ?? string.Empty))
                return null;

            //return await _dbProvider.ExecuteQueryAsync<OrderDto>(container, $"select * from orders where userId={request.UserId}");
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
    }
}
