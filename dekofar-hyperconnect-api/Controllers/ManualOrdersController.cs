using Dekofar.HyperConnect.Application.ManualOrders.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dekofar.HyperConnect.API.Controllers
{
    [ApiController]
    [Route("api/manual-orders")]
    public class ManualOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ManualOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateManualOrderCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _mediator.Send(command);
            return Ok(id);
        }
    }
}

