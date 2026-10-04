using MediatR;
using MicroserviceApp.Inventory.Application.RequestHandlers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MicroserviceApp.Inventory.Api.Controllers
{
    public class InventoryController : Controller
    {
        public IMediator _mediator { get; }
        public ILogger<InventoryController> _logger { get; }

        public InventoryController(IMediator mediator, ILogger<InventoryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GetInventoryItem([FromBody] GetInventoryItem request)
        {
            if (request == null || request.ProductId == Guid.Empty)
                return BadRequest("Invalid product id.");

            var response = await _mediator.Send(request);
            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AdjustStock([FromBody] AdjustStock request)
        {
            if (request == null || request.ProductId == Guid.Empty)
            {
                _logger.LogWarning($"Invalid stock adjustment request received. Request: {JsonSerializer.Serialize(request)}");
                return BadRequest("Invalid inventory info.");
            }

            var response = await _mediator.Send(request);
            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReserveInventory([FromBody] ReserveInventory request)
        {
            if (request == null || request.OrderId == Guid.Empty || (!request.Products?.Any() ?? true))
            {
                _logger.LogWarning($"Invalid inventory reservation request received. Request: {JsonSerializer.Serialize(request)}");
                return BadRequest("Invalid reservation info.");
            }

            var response = await _mediator.Send(request);
            if (response == null)
                return NotFound();

            _logger.LogInformation($"Inventory reservation successfull, reservation id: {response.Id}");
            return Ok(response);
        }
    }
}
