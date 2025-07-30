using Dekofar.HyperConnect.Application.Common.Interfaces;
using Dekofar.HyperConnect.Domain.Entities.support;
using Dekofar.HyperConnect.Domain.Entities.Support;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dekofar.HyperConnect.Application.Support.Commands.AddTicketNote
{
    public class AddTicketNoteCommandHandler : IRequestHandler<AddTicketNoteCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public AddTicketNoteCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(AddTicketNoteCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _context.SupportTickets
                .FirstOrDefaultAsync(x => x.Id == request.TicketId, cancellationToken);

            if (ticket == null) return false;

            var note = new TicketNote
            {
                TicketId = ticket.Id,
                Message = request.Message,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _currentUser.UserId ?? Guid.Empty
            };

            await _context.TicketNotes.AddAsync(note, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
