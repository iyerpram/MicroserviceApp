using MediatR;
using MicroserviceApp.Notification.Application.Dto;

namespace MicroserviceApp.Notification.Application.RequestHandlers
{
    public class SendNotification : IRequest<NotificationDto>
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? EventType { get; set; }
        public Guid? RelatedEntityId { get; set; }
    }

    public class SendNotificationHandler : IRequestHandler<SendNotification, NotificationDto>
    {
        public INotificationRepository _notificationRepository { get; }

        public SendNotificationHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<NotificationDto> Handle(SendNotification request, CancellationToken cancellationToken)
        {
            if (request.UserId <= 0 && string.IsNullOrWhiteSpace(request.Email))
                return null;

            return await _notificationRepository.CreateNotificationAsync(request);
        }
    }
}
