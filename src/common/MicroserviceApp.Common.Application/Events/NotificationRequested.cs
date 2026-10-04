namespace MicroserviceApp.Common.Application.Events
{
    public class NotificationRequested
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? EventType { get; set; }
        public Guid? RelatedEntityId { get; set; }
    }
}
