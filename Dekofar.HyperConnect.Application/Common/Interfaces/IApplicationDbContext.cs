using Dekofar.HyperConnect.Domain.Entities.Support;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<SupportTicket> SupportTickets { get; }
        DbSet<TicketNote> TicketNotes { get; }
        DbSet<TicketLog> TicketLogs { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
