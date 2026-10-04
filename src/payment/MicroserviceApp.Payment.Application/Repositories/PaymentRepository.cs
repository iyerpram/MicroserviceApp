using AutoMapper;
using MicroserviceApp.Common.Abstractions.Database;
using MicroserviceApp.Common.Domain.Models;
using MicroserviceApp.Payment.Application.Dto;
using MicroserviceApp.Payment.Application.RequestHandlers;
using Microsoft.Extensions.Configuration;

namespace MicroserviceApp.Payment.Application
{
    public interface IPaymentRepository
    {
        Task<PaymentDto> GetPaymentByIdAsync(Guid paymentId);
        Task<IEnumerable<PaymentDto>> GetPaymentsByOrderIdAsync(Guid orderId);
        Task<PaymentDto> CreatePaymentAsync(ProcessPayment request);
    }

    public class PaymentRepository : IPaymentRepository
    {
        private IExtendedDbProvider<Domain.Models.Payment> _dbProvider { get; }
        public IConfiguration _configuration { get; }
        public IMapper _mapper { get; }

        public PaymentRepository(IExtendedDbProvider<Domain.Models.Payment> dbProvider, IConfiguration configuration, IMapper mapper)
        {
            _dbProvider = dbProvider;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<PaymentDto> GetPaymentByIdAsync(Guid paymentId)
        {
            var payment = await _dbProvider.GetItemAsync(paymentId.ToString());
            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByOrderIdAsync(Guid orderId)
        {
            return await _dbProvider.ExecuteQueryAsync<PaymentDto>($"select * from payments where orderId={orderId}");
        }

        public async Task<PaymentDto> CreatePaymentAsync(ProcessPayment request)
        {
            var payment = new Domain.Models.Payment
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderId,
                Amount = request.Amount,
                User = _mapper.Map<User>(request.User),
                Status = request.Amount > 0 ? "Completed" : "Failed"
            };

            var isAdded = await _dbProvider.CreateItemAsync(payment);
            return isAdded ? _mapper.Map<PaymentDto>(payment) : null;
        }
    }
}
