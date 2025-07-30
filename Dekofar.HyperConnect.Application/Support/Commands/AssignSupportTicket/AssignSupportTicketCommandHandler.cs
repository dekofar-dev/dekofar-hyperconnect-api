using Dekofar.HyperConnect.Application.Common.Interfaces;
using Dekofar.HyperConnect.Domain.Entities.support;
using Dekofar.HyperConnect.Domain.Entities.Support;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dekofar.HyperConnect.Application.Support.Commands.AssignSupportTicket
{
    public class AssignSupportTicketCommandHandler : IRequestHandler<AssignSupportTicketCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public AssignSupportTicketCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(AssignSupportTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _context.SupportTickets
                .FirstOrDefaultAsync(x => x.Id == request.TicketId, cancellationToken);

            if (ticket == null)
                return false;

            // Yeni Assigned değeri Guid tipine çevrilmeli
            if (!Guid.TryParse(request.NewAssignedUserId, out var newAssignedGuid))
                return false;

            var oldAssigned = ticket.AssignedToUserId;
            ticket.AssignedToUserId = newAssignedGuid;

            // Geçmiş kaydı (eğer değiştiyse)
            if (oldAssigned != newAssignedGuid)
            {
                var history = new SupportTicketHistory
                {
                    TicketId = ticket.Id,
                    FieldChanged = "AssignedToUserId",
                    OldValue = oldAssigned?.ToString(),
                    NewValue = newAssignedGuid.ToString(),
                    ChangedAt = DateTime.UtcNow,
                    ChangedBy = _currentUser.UserId ?? Guid.Empty
                };

                await _context.SupportTicketHistories.AddAsync(history, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
