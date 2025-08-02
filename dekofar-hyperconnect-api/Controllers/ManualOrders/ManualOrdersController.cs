using Dekofar.HyperConnect.Application.ManualOrders.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dekofar.API.Controllers
{
    [ApiController]
    [Route("api/manual-orders")]
    // Manuel sipariş işlemlerini yöneten controller
    public class ManualOrdersController : ControllerBase
    {
        // MediatR aracısı
        private readonly IMediator _mediator;

        // MediatR'ı alan kurucu metot
        public ManualOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Yeni manuel sipariş oluşturur
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

