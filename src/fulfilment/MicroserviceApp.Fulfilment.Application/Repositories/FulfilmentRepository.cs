using AutoMapper;
using MicroserviceApp.Common.Abstractions.Database;
using MicroserviceApp.Common.Domain.Models;
using MicroserviceApp.Fulfilment.Application.Dto;
using MicroserviceApp.Fulfilment.Application.RequestHandlers;
using Microsoft.Extensions.Configuration;

namespace MicroserviceApp.Fulfilment.Application
{
    public interface IFulfilmentRepository
    {
        Task<FulfilmentDto> GetFulfilmentByOrderIdAsync(Guid orderId);
        Task<FulfilmentDto> CreateFulfilmentAsync(CreateFulfilment request);
        Task<FulfilmentDto> ShipFulfilmentAsync(ShipFulfilment request);
    }

    public class FulfilmentRepository : IFulfilmentRepository
    {
        private IExtendedDbProvider<Domain.Models.Fulfilment> _dbProvider { get; }
        public IConfiguration _configuration { get; }
        public IMapper _mapper { get; }

        public FulfilmentRepository(IExtendedDbProvider<Domain.Models.Fulfilment> dbProvider, IConfiguration configuration, IMapper mapper)
        {
            _dbProvider = dbProvider;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<FulfilmentDto> GetFulfilmentByOrderIdAsync(Guid orderId)
        {
            var items = await _dbProvider.ExecuteQueryAsync<FulfilmentDto>($"select * from fulfilments where orderId={orderId}");
            return items.FirstOrDefault();
        }

        public async Task<FulfilmentDto> CreateFulfilmentAsync(CreateFulfilment request)
        {
            var fulfilment = new Domain.Models.Fulfilment
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderId,
                User = _mapper.Map<User>(request.User),
                Status = "Created",
                TrackingNumber = null
            };

            var isAdded = await _dbProvider.CreateItemAsync(fulfilment);
            return isAdded ? _mapper.Map<FulfilmentDto>(fulfilment) : null;
        }

        public async Task<FulfilmentDto> ShipFulfilmentAsync(ShipFulfilment request)
        {
            var fulfilment = await _dbProvider.GetItemAsync(request.FulfilmentId.ToString());
            if (fulfilment == null)
                return null;

            fulfilment.Status = "Shipped";
            fulfilment.TrackingNumber = string.IsNullOrWhiteSpace(request.TrackingNumber)
                ? Guid.NewGuid().ToString("N")[..12].ToUpperInvariant()
                : request.TrackingNumber;

            var isUpdated = await _dbProvider.UpdateItemAsync(request.FulfilmentId.ToString(), fulfilment);
            return isUpdated ? _mapper.Map<FulfilmentDto>(fulfilment) : null;
        }
    }
}
