using MediatR;
using MicroserviceApp.Fulfilment.Application.RequestHandlers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MicroserviceApp.Fulfilment.Api.Controllers
{
    public class FulfilmentController : Controller
    {
        public IMediator _mediator { get; }
        public ILogger<FulfilmentController> _logger { get; }

        public FulfilmentController(IMediator mediator, ILogger<FulfilmentController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GetFulfilmentByOrderId([FromBody] GetFulfilmentByOrderId request)
        {
            if (request == null || request.OrderId == Guid.Empty)
                return BadRequest("Invalid order id.");

            var response = await _mediator.Send(request);
            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFulfilment([FromBody] CreateFulfilment request)
        {
            if (request?.User == null || request.OrderId == Guid.Empty)
            {
                _logger.LogWarning($"Invalid fulfilment request received. Request: {JsonSerializer.Serialize(request)}");
                return BadRequest("Invalid fulfilment info.");
            }

            var response = await _mediator.Send(request);
            if (response == null)
                return NotFound();

            _logger.LogInformation($"Fulfilment creation successfull, fulfilment id: {response.Id}");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ShipFulfilment([FromBody] ShipFulfilment request)
        {
            if (request == null || request.FulfilmentId == Guid.Empty)
                return BadRequest("Invalid fulfilment id.");

            var response = await _mediator.Send(request);
            if (response == null)
                return NotFound();

            _logger.LogInformation($"Fulfilment shipped, tracking: {response.TrackingNumber}");
            return Ok(response);
        }
    }
}
