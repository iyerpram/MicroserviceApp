using MediatR;
using MicroserviceApp.Payment.Application.RequestHandlers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MicroserviceApp.Payment.Api.Controllers
{
    public class PaymentsController : Controller
    {
        public IMediator _mediator { get; }
        public ILogger<PaymentsController> _logger { get; }

        public PaymentsController(IMediator mediator, ILogger<PaymentsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GetPaymentById([FromBody] GetPaymentById request)
        {
            if (request == null || request.PaymentId == Guid.Empty)
                return BadRequest("Invalid payment id.");

            var response = await _mediator.Send(request);
            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetPaymentsByOrderId([FromBody] GetPaymentsByOrderId request)
        {
            if (request == null || request.OrderId == Guid.Empty)
                return BadRequest("Invalid order id.");

            var response = await _mediator.Send(request);
            if (!response?.Any() ?? true)
                return NotFound();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPayment request)
        {
            if (request?.User == null || request.OrderId == Guid.Empty)
            {
                _logger.LogWarning($"Invalid payment request received. Request: {JsonSerializer.Serialize(request)}");
                return BadRequest("Invalid payment info.");
            }

            var response = await _mediator.Send(request);
            if (response == null)
            {
                _logger.LogWarning($"Payment processing failed. Request: {JsonSerializer.Serialize(request)}");
                return NotFound();
            }

            _logger.LogInformation($"Payment processing successfull, payment id: {response.Id}");
            return Ok(response);
        }
    }
}
