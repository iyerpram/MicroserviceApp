using MediatR;
using MicroserviceApp.Cart.Application.RequestHandlers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MicroserviceApp.Orders.Api.Controllers
{
    public class CartController : Controller
    {
        public IMediator _mediator { get; }
        public ILogger<CartController> _logger { get; }

        public CartController(IMediator mediator, ILogger<CartController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GetItems([FromBody] GetCartItems request)
        {
            if (request?.User == null)
                return BadRequest("Invalid user id.");

            var response = await _mediator.Send(request);
            if (response == null || (response?.Products?.Any() ?? false))
                return NotFound();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToCart([FromBody] AddItemToCart request)
        {
            if (request?.User == null || request?.Product == null)
            {
                _logger.LogWarning($"Invalid request received. Request: {JsonSerializer.Serialize(request)}");
                return BadRequest("Invalid request.");
            }

            var response = await _mediator.Send(request);
            if (!response)
            {
                _logger.LogWarning($"Request failed. Request: {JsonSerializer.Serialize(request)}");
                return NotFound();
            }

            _logger.LogInformation($"Request successfull.");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CreateOrder request)
        {
            if (request?.User == null || (!request?.Products?.Any() ?? true))
            {
                _logger.LogWarning($"Invalid Cart checkout request received. Request: {JsonSerializer.Serialize(request)}");
                return BadRequest("Invalid cart info.");
            }

            var response = await _mediator.Send(request);
            if (!response)
            {
                _logger.LogWarning($"Cart checkout failed. Request: {JsonSerializer.Serialize(request)}");
                return NotFound();
            }

            _logger.LogInformation($"Cart checkout successfull.");
            return Ok(response);
        }
    }
}
