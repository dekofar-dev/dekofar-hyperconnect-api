using Dekofar.HyperConnect.Application.Discounts.Commands;
using Dekofar.HyperConnect.Application.Discounts.DTOs;
using Dekofar.HyperConnect.Application.Discounts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.API.Controllers
{
    [ApiController]
    [Route("api/discounts")]
    public class DiscountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DiscountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<DiscountDto>>> GetAll()
        {
            var discounts = await _mediator.Send(new GetAllDiscountsQuery());
            return Ok(discounts);
        }

        [HttpGet("active")]
        public async Task<ActionResult<List<DiscountDto>>> GetActive()
        {
            var discounts = await _mediator.Send(new GetAllDiscountsQuery(true));
            return Ok(discounts);
        }

        [HttpPost]
        [Authorize(Policy = "CanManageDiscounts")]
        public async Task<IActionResult> Create([FromBody] CreateDiscountCommand command)
        {
            // Only users with the manage discounts permission can create
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "CanManageDiscounts")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDiscountCommand command)
        {
            // Policy prevents unauthorized modifications
            if (id != command.Id) return BadRequest();
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "CanManageDiscounts")]
        public async Task<IActionResult> Delete(Guid id)
        {
            // Policy ensures only authorized users can delete
            await _mediator.Send(new DeleteDiscountCommand(id));
            return Ok();
        }
    }
}
