using MediatR;
using MicroserviceApp.Orders.Application.RequestHandlers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MicroserviceApp.Orders.Api.Controllers
{
    public class OrdersController : Controller
    {
        public IMediator _mediator { get; }
        public ILogger<OrdersController> _logger { get; }

        public OrdersController(IMediator mediator, ILogger<OrdersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GetOrders([FromBody] GetOrders request)
        {
            if (string.IsNullOrWhiteSpace(request?.UserId))
                return BadRequest("Invalid user id.");

            var response = await _mediator.Send(request);
            if (!response?.Any() ?? true)
                return NotFound();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetOrderById(GetOrderById request)
        {
            if (request == null)
                return BadRequest("Invalid order id.");

            var response = await _mediator.Send(request);
            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrder request)
        {
            if (request?.User == null || (!request?.Products?.Any() ?? true))
            {
                _logger.LogWarning($"Invalid order creation request received. Request: {JsonSerializer.Serialize(request)}");
                return BadRequest("Invalid order info.");
            }

            var response = await _mediator.Send(request);
            if (response == null)
            {
                _logger.LogWarning($"Order creation failed. Request: {JsonSerializer.Serialize(request)}");
                return NotFound();
            }

            _logger.LogInformation($"Order creation successfull, order id: {response.Id}");
            return Ok(response);
        }
    }
}
