using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Dekofar.HyperConnect.Application.Users.Commands;
using Dekofar.HyperConnect.Application.Users.DTOs;
using Dekofar.HyperConnect.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dekofar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /api/users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var users = await _mediator.Send(new GetAllUsersWithRolesQuery());
            return Ok(users);
        }

        // POST /api/users/{id}/roles
        [HttpPost("{id:guid}/roles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRoles(Guid id, [FromBody] List<string> roles)
        {
            var result = await _mediator.Send(new AssignRolesToUserCommand { UserId = id, Roles = roles });
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        // POST /api/users/{id}/avatar
        [HttpPost("{id:guid}/avatar")]
        [Authorize]
        public async Task<IActionResult> UploadAvatar(Guid id, [FromForm] IFormFile file)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null || currentUserId != id.ToString())
            {
                return Forbid();
            }

            var url = await _mediator.Send(new UploadProfileImageCommand { UserId = id, File = file });
            return Ok(new { avatarUrl = url });
        }
    }
}
