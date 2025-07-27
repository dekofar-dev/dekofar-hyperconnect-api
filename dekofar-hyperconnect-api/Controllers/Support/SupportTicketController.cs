using Dekofar.HyperConnect.Application.Support.Commands.CreateSupportTicket;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.API.Controllers.Support
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Token zorunlu
    public class SupportTicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SupportTicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Yeni destek talebi oluşturur
        /// </summary>
        /// <param name="command">Destek talebi oluşturma verisi</param>
        /// <returns>Oluşturulan ticket Id</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSupportTicketCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _mediator.Send(command);
            return Ok(new { id });
        }
    }
}
