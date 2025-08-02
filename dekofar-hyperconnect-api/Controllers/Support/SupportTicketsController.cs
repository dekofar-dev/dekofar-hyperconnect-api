using Dekofar.HyperConnect.Application.SupportTickets.Commands;
using Dekofar.HyperConnect.Application.SupportTickets.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dekofar.API.Controllers
{
    [ApiController]
    [Route("api/support-tickets")]
    [Authorize]
    // Destek talebi işlemlerini yöneten controller
    public class SupportTicketsController : ControllerBase
    {
        // MediatR aracısı
        private readonly IMediator _mediator;

        // MediatR bağımlılığını alan kurucu
        public SupportTicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Yeni destek talebi oluşturur
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateSupportTicketCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        // Kullanıcının kendi taleplerini döner
        [HttpGet("my")]
        public async Task<IActionResult> GetMyTickets()
        {
            var tickets = await _mediator.Send(new GetMyTicketsQuery());
            return Ok(tickets);
        }

        // Belirli bir talebi id ile getirir
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var ticket = await _mediator.Send(new GetTicketByIdQuery(id));
            if (ticket == null) return NotFound();
            return Ok(ticket);
        }

        // Destek talebini bir personele atar
        [HttpPost("{id}/assign")]
        [Authorize(Policy = "CanAssignTicket")]
        public async Task<IActionResult> Assign(Guid id, [FromBody] AssignSupportTicketCommand command)
        {
            if (id != command.TicketId) return BadRequest();
            await _mediator.Send(command);
            return Ok();
        }

        // Talebin durumunu günceller
        [HttpPost("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateSupportTicketStatusCommand command)
        {
            if (id != command.TicketId) return BadRequest();
            await _mediator.Send(command);
            return Ok();
        }
    }
}
