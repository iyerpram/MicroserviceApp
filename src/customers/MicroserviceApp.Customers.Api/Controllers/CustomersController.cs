using MediatR;
using MicroserviceApp.Customers.Application.RequestHandlers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MicroserviceApp.Customers.Api.Controllers
{
    public class CustomersController : Controller
    {
        public IMediator _mediator { get; }
        public ILogger<CustomersController> _logger { get; }

        public CustomersController(IMediator mediator, ILogger<CustomersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GetCustomers([FromBody] GetCustomers request)
        {
            var response = await _mediator.Send(request ?? new GetCustomers());
            if (!response?.Any() ?? true)
                return NotFound();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetCustomerById([FromBody] GetCustomerById request)
        {
            if (request == null || request.CustomerId == Guid.Empty)
                return BadRequest("Invalid customer id.");

            var response = await _mediator.Send(request);
            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomer request)
        {
            if (request?.User == null)
            {
                _logger.LogWarning($"Invalid customer creation request received. Request: {JsonSerializer.Serialize(request)}");
                return BadRequest("Invalid customer info.");
            }

            var response = await _mediator.Send(request);
            if (response == null)
            {
                _logger.LogWarning($"Customer creation failed. Request: {JsonSerializer.Serialize(request)}");
                return NotFound();
            }

            _logger.LogInformation($"Customer creation successfull, customer id: {response.Id}");
            return Ok(response);
        }
    }
}
