using System;
using System.Threading.Tasks;
using Dekofar.HyperConnect.Application.UserMessages.Commands;
using Dekofar.HyperConnect.Application.UserMessages.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dekofar.API.Controllers
{
    [ApiController]
    [Route("api/usermessages")]
    [Authorize]
    public class UserMessagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserMessagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("chat/{userId}")]
        public async Task<IActionResult> GetChat(Guid userId)
        {
            var messages = await _mediator.Send(new GetChatMessagesQuery(userId));
            return Ok(messages);
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromForm] SendUserMessageCommand command)
        {
            var message = await _mediator.Send(command);
            return Ok(message);
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var count = await _mediator.Send(new GetUnreadMessageCountQuery());
            return Ok(count);
        }

        [HttpPut("mark-as-read/{chatUserId}")]
        public async Task<IActionResult> MarkAsRead(Guid chatUserId)
        {
            var updated = await _mediator.Send(new MarkMessagesAsReadCommand(chatUserId));
            return Ok(updated);
        }
    }
}
