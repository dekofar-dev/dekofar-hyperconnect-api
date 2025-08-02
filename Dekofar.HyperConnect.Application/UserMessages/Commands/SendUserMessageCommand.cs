using System;
using Dekofar.HyperConnect.Application.UserMessages.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Dekofar.HyperConnect.Application.UserMessages.Commands
{
    public class SendUserMessageCommand : IRequest<UserMessageDto>
    {
        public Guid ReceiverId { get; set; }
        public string? Text { get; set; }
        public IFormFile? File { get; set; }
    }
}
