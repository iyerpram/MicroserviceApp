namespace MicroserviceApp.Notification.Domain.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? EventType { get; set; }
        public Guid? RelatedEntityId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
