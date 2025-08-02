using Dekofar.HyperConnect.Application.OrderCommissions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.API.Controllers
{
    [ApiController]
    [Route("api/order-commissions")]
    [Authorize]
    public class OrderCommissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderCommissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetForCurrentUser()
        {
            var commissions = await _mediator.Send(new GetCommissionsByUserQuery());
            return Ok(commissions);
        }

        [HttpGet("user/total")]
        public async Task<IActionResult> GetTotalForCurrentUser()
        {
            var total = await _mediator.Send(new GetUserTotalCommissionQuery());
            return Ok(total);
        }
    }
}
