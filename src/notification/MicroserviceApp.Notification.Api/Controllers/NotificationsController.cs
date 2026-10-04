using MediatR;
using MicroserviceApp.Notification.Application.RequestHandlers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MicroserviceApp.Notification.Api.Controllers
{
    public class NotificationsController : Controller
    {
        public IMediator _mediator { get; }
        public ILogger<NotificationsController> _logger { get; }

        public NotificationsController(IMediator mediator, ILogger<NotificationsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GetNotifications([FromBody] GetNotifications request)
        {
            if (request == null || request.UserId <= 0)
                return BadRequest("Invalid user id.");

            var response = await _mediator.Send(request);
            if (!response?.Any() ?? true)
                return NotFound();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody] SendNotification request)
        {
            if (request == null || (request.UserId <= 0 && string.IsNullOrWhiteSpace(request.Email)))
            {
                _logger.LogWarning($"Invalid notification request received. Request: {JsonSerializer.Serialize(request)}");
                return BadRequest("Invalid notification info.");
            }

            var response = await _mediator.Send(request);
            if (response == null)
                return NotFound();

            _logger.LogInformation($"Notification sent, notification id: {response.Id}");
            return Ok(response);
        }
    }
}
