using Dekofar.HyperConnect.Application.Roles.Commands.CreateRole;
using Dekofar.HyperConnect.Application.Roles.Queries.GetRoles;
using Dekofar.HyperConnect.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dekofar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RoleName))
            {
                return BadRequest("Role name cannot be empty.");
            }

            var result = await _mediator.Send(new CreateRoleCommand(request.RoleName));
            if (result.Succeeded)
            {
                return Ok($"'{request.RoleName}' role created successfully.");
            }

            return BadRequest(result.Errors);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _mediator.Send(new GetRolesQuery());
            return Ok(roles);
        }
    }
}
