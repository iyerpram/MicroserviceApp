using MediatR;
using MicroserviceApp.Common.Abstractions.Database;
using MicroserviceApp.Orders.Application.Dto;
using Microsoft.Extensions.Configuration;

namespace MicroserviceApp.Orders.Application.RequestHandlers
{
    public class GetOrderById : IRequest<OrderDto>
    {
        public Guid OrderId { get; set; }
    }

    public class GetOrderByIdHandler : IRequestHandler<GetOrderById, OrderDto>
    {
        private IDbProvider _dbProvider { get; }
        public IConfiguration _configuration { get; }

        private string _container => _configuration?["Database:Container"] ?? string.Empty;
        public GetOrderByIdHandler(IDbProvider dbProvider, IConfiguration configuration)
        {
            _dbProvider = dbProvider;
            _configuration = configuration;
        }

        public async Task<OrderDto> Handle(GetOrderById request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request?.OrderId.ToString() ?? string.Empty))
                return null;

            //return await _dbProvider.GetItemAsync<OrderDto>(container, request.OrderId.ToString());

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
    }
