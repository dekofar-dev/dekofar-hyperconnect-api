using Dekofar.HyperConnect.Application.SupportTickets.Commands;
using Dekofar.HyperConnect.Application.SupportTickets.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.API.Controllers
{
    [ApiController]
    [Route("api/support-tickets")]
    [Authorize]
    public class SupportTicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SupportTicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateSupportTicketCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyTickets()
        {
            var tickets = await _mediator.Send(new GetMyTicketsQuery());
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var ticket = await _mediator.Send(new GetTicketByIdQuery(id));
            if (ticket == null) return NotFound();
            return Ok(ticket);
        }

        [HttpPost("{id}/assign")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Assign(Guid id, [FromBody] AssignSupportTicketCommand command)
        {
            if (id != command.TicketId) return BadRequest();
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateSupportTicketStatusCommand command)
        {
            if (id != command.TicketId) return BadRequest();
            await _mediator.Send(command);
            return Ok();
        }
    }
}
