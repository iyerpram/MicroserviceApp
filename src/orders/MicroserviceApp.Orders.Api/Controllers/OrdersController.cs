using MediatR;
using MicroserviceApp.Orders.Application.RequestHandlers;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceApp.Orders.Api.Controllers
{
    public class OrdersController : Controller
    {
        public IMediator _mediator { get; }

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> GetOrders([FromBody] GetOrders request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetOrderById(GetOrderById request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
