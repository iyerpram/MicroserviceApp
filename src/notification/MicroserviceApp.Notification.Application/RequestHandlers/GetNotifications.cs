using MediatR;
using MicroserviceApp.Notification.Application.Dto;

namespace MicroserviceApp.Notification.Application.RequestHandlers
{
    public class GetNotifications : IRequest<IEnumerable<NotificationDto>>
    {
        public int UserId { get; set; }
    }

    public class GetNotificationsHandler : IRequestHandler<GetNotifications, IEnumerable<NotificationDto>>
    {
        public INotificationRepository _notificationRepository { get; }

        public GetNotificationsHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<NotificationDto>> Handle(GetNotifications request, CancellationToken cancellationToken)
        {
            if (request.UserId <= 0)
                return null;

            return await _notificationRepository.GetNotificationsAsync(request.UserId);
        }
    }
}
