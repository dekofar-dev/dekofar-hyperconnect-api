using Dekofar.HyperConnect.Application.Common.Interfaces;
using Dekofar.HyperConnect.Domain.Entities.Support;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Application.Support.Commands.CreateSupportTicket
{
    public class CreateSupportTicketCommandHandler : IRequestHandler<CreateSupportTicketCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public CreateSupportTicketCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<Guid> Handle(CreateSupportTicketCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.UserId == null)
                throw new UnauthorizedAccessException("Kullanıcı doğrulanamadı.");

            var userId = _currentUser.UserId.Value;

            var ticket = new SupportTicket
            {
                Id = Guid.NewGuid(),
                Subject = request.Subject,
                Description = request.Description,
                Category = (SupportCategory)request.Category,
                Priority = request.Priority.HasValue
                    ? (SupportPriority)request.Priority.Value
                    : SupportPriority.Orta,
                Tags = request.Tags,
                ShopifyOrderId = request.ShopifyOrderId,
                CustomerPhone = request.CustomerPhone,
                CustomerEmail = request.CustomerEmail,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId
            };

            await _context.SupportTickets.AddAsync(ticket, cancellationToken);

            var log = new TicketLog
            {
                Id = Guid.NewGuid(),
                TicketId = ticket.Id,
                Action = "Destek talebi oluşturuldu.",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId
            };

            await _context.TicketLogs.AddAsync(log, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return ticket.Id;
        }
    }
}
