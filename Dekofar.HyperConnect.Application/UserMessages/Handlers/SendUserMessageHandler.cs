using System;
using System.Threading;
using System.Threading.Tasks;
using Dekofar.HyperConnect.Application.Common.Interfaces;
using Dekofar.HyperConnect.Application.UserMessages.Commands;
using Dekofar.HyperConnect.Application.UserMessages.DTOs;
using Dekofar.HyperConnect.Domain.Entities;
using MediatR;

namespace Dekofar.HyperConnect.Application.UserMessages.Handlers
{
    public class SendUserMessageHandler : IRequestHandler<SendUserMessageCommand, UserMessageDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileStorageService _fileStorageService;

        public SendUserMessageHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IFileStorageService fileStorageService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _fileStorageService = fileStorageService;
        }

        public async Task<UserMessageDto> Handle(SendUserMessageCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId == null)
                throw new UnauthorizedAccessException();

            string? attachmentUrl = null;
            if (request.File != null)
            {
                attachmentUrl = await _fileStorageService.SaveChatAttachmentAsync(request.File, _currentUserService.UserId.Value);
            }

            var message = new UserMessage
            {
                Id = Guid.NewGuid(),
                SenderId = _currentUserService.UserId.Value,
                ReceiverId = request.ReceiverId,
                Text = request.Text,
                AttachmentUrl = attachmentUrl,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            await _context.UserMessages.AddAsync(message, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new UserMessageDto
            {
                Id = message.Id,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                Text = message.Text,
                AttachmentUrl = message.AttachmentUrl,
                SentAt = message.SentAt,
                IsRead = message.IsRead,
                ReadAt = message.ReadAt
            };
        }
    }
}
