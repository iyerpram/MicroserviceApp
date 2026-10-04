using MediatR;
using MicroserviceApp.Common.Abstractions.Messaging;
using MicroserviceApp.Common.Application;
using MicroserviceApp.Common.Application.Events;
using MicroserviceApp.Payment.Application.Dto;

namespace MicroserviceApp.Payment.Application.RequestHandlers
{
    public class ProcessPayment : IRequest<PaymentDto>
    {
        public Guid OrderId { get; set; }
        public UserDto? User { get; set; }
        public IEnumerable<ProductDto>? Products { get; set; }
        public decimal Amount { get; set; }
    }

    public class ProcessPaymentHandler : IRequestHandler<ProcessPayment, PaymentDto>
    {
        public IPaymentRepository _paymentRepository { get; }
        public IMessagingProviderFactory _messagingProviderFactory { get; }

        public ProcessPaymentHandler(IPaymentRepository paymentRepository, IMessagingProviderFactory messagingProviderFactory)
        {
            _paymentRepository = paymentRepository;
            _messagingProviderFactory = messagingProviderFactory;
        }

        public async Task<PaymentDto> Handle(ProcessPayment request, CancellationToken cancellationToken)
        {
            if (request.OrderId == Guid.Empty || request.User == null)
                return null;

            if (request.Amount <= 0)
                request.Amount = request.Products?.Sum(p => p.Price) ?? 0;

            var payment = await _paymentRepository.CreatePaymentAsync(request);
            if (payment == null)
                return null;

            var paymentProcessed = new PaymentProcessed
            {
                PaymentId = payment.Id,
                OrderId = payment.OrderId,
                User = payment.User,
                Amount = payment.Amount,
                Status = payment.Status,
                Products = request.Products
            };

            var inventoryProvider = _messagingProviderFactory.GetMessagingProvider(MessagingProviderType.AzureServiceBus, "Inventory");
            await inventoryProvider.PublishMessageAsync("Payment Processed", paymentProcessed);

            var ordersProvider = _messagingProviderFactory.GetMessagingProvider(MessagingProviderType.AzureServiceBus, "Order");
            await ordersProvider.PublishMessageAsync("Payment Processed", paymentProcessed);

            var notificationProvider = _messagingProviderFactory.GetMessagingProvider(MessagingProviderType.AzureServiceBus, "Notification");
            await notificationProvider.PublishMessageAsync("Notification Requested", new NotificationRequested
            {
                UserId = request.User.Id,
                Email = request.User.Email,
                EventType = nameof(PaymentProcessed),
                RelatedEntityId = payment.Id,
                Subject = "Payment processed",
                Body = $"Payment {payment.Id} for order {payment.OrderId} is {payment.Status}."
            });

            return payment;
        }
    }
}
