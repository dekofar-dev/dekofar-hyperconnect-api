using Dekofar.HyperConnect.Application.Common.Interfaces;
using Dekofar.HyperConnect.Domain.Entities.support;
using Dekofar.HyperConnect.Domain.Entities.Support;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dekofar.HyperConnect.Application.Support.Commands.UpdateTicketStatus
{
    public class UpdateTicketStatusCommandHandler : IRequestHandler<UpdateTicketStatusCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public UpdateTicketStatusCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(UpdateTicketStatusCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _context.SupportTickets.FirstOrDefaultAsync(x => x.Id == request.TicketId, cancellationToken);
            if (ticket == null) return false;

            var oldStatus = ticket.Status;
            ticket.Status = (SupportStatus)request.NewStatus;

            _context.SupportTicketHistories.Add(new SupportTicketHistory
            {
                TicketId = ticket.Id,
                FieldChanged = "Status",
                OldValue = oldStatus.ToString(),
                NewValue = ticket.Status.ToString(),
                ChangedAt = DateTime.UtcNow,
                ChangedBy = _currentUser.UserId ?? Guid.Empty
            });

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
