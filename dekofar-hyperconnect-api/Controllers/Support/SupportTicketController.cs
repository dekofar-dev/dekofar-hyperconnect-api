using Dekofar.HyperConnect.Application.Support.Commands.AddTicketNote;
using Dekofar.HyperConnect.Application.Support.Commands.AssignSupportTicket;
using Dekofar.HyperConnect.Application.Support.Commands.CreateSupportTicket;
using Dekofar.HyperConnect.Application.Support.Commands.Queries.GetAllSupportTickets;
using Dekofar.HyperConnect.Application.Support.Commands.UpdateTicketStatus;
using Dekofar.HyperConnect.Application.Support.Queries.GetAllSupportTickets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.API.Controllers.Support
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SupportTicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SupportTicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Yeni destek talebi oluşturur (dosya eklenebilir).
        /// </summary>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateSupportTicketCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Tüm destek taleplerini getirir.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllSupportTicketsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Belirli destek talebini detaylı getirir.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetSupportTicketByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Destek talebine not ekler.
        /// </summary>
        [HttpPost("{id}/note")]
        public async Task<IActionResult> AddNote(int id, [FromBody] AddTicketNoteCommand command)
        {
            command.TicketId = id;
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Talebi belirli bir kullanıcıya atar.
        /// </summary>
        [HttpPost("{id}/assign")]
        public async Task<IActionResult> Assign(int id, [FromBody] AssignSupportTicketCommand command)
        {
            if (id != command.TicketId) return BadRequest("ID uyuşmuyor.");
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Talep durumunu günceller.
        /// </summary>
        [HttpPost("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateTicketStatusCommand command)
        {
            if (id != command.TicketId) return BadRequest("ID uyuşmuyor.");
            await _mediator.Send(command);
            return Ok();
        }
    }
}
