using Dekofar.HyperConnect.Application.Notes.Commands;
using Dekofar.HyperConnect.Application.Notes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.API.Controllers
{
    [ApiController]
    [Route("api/notes")]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{targetType}/{targetId}")]
        public async Task<IActionResult> GetByTarget(string targetType, Guid targetId)
        {
            var notes = await _mediator.Send(new GetNotesByTargetQuery(targetType, targetId));
            return Ok(notes);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddNoteCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }
    }
}
