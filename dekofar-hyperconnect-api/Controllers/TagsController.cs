using Dekofar.HyperConnect.Application.Features.Tags.Commands;
using Dekofar.HyperConnect.Application.Features.Tags.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace dekofar_hyperconnect_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<TagDto>>> GetAll()
        {
            return await _mediator.Send(new GetAllTagsQuery());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TagDto>> GetById(int id)
        {
            return await _mediator.Send(new GetTagByIdQuery { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateTagCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTagCommand command)
        {
            if (id != command.Id)
                return BadRequest();

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteTagCommand { Id = id });
            return NoContent();
        }
    }

}
