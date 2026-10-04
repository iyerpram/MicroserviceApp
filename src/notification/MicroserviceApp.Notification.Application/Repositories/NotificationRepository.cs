using AutoMapper;
using MicroserviceApp.Common.Abstractions.Database;
using MicroserviceApp.Notification.Application.Dto;
using MicroserviceApp.Notification.Application.RequestHandlers;
using Microsoft.Extensions.Configuration;

namespace MicroserviceApp.Notification.Application
{
    public interface INotificationRepository
    {
        Task<IEnumerable<NotificationDto>> GetNotificationsAsync(int userId);
        Task<NotificationDto> CreateNotificationAsync(SendNotification request);
    }

    public class NotificationRepository : INotificationRepository
    {
        private IExtendedDbProvider<Domain.Models.Notification> _dbProvider { get; }
        public IConfiguration _configuration { get; }
        public IMapper _mapper { get; }

        public NotificationRepository(IExtendedDbProvider<Domain.Models.Notification> dbProvider, IConfiguration configuration, IMapper mapper)
        {
            _dbProvider = dbProvider;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NotificationDto>> GetNotificationsAsync(int userId)
        {
            return await _dbProvider.ExecuteQueryAsync<NotificationDto>($"select * from notifications where userId={userId}");
        }

        public async Task<NotificationDto> CreateNotificationAsync(SendNotification request)
        {
            var notification = new Domain.Models.Notification
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Email = request.Email,
                Subject = request.Subject,
                Body = request.Body,
                EventType = request.EventType,
                RelatedEntityId = request.RelatedEntityId,
                CreatedAt = DateTime.UtcNow
            };

            var isAdded = await _dbProvider.CreateItemAsync(notification);
            return isAdded ? _mapper.Map<NotificationDto>(notification) : null;
        }
    }
}
