using Dekofar.Domain.Entities;
using Dekofar.HyperConnect.Domain.Entities.support;
using Dekofar.HyperConnect.Domain.Entities.Support;
using Dekofar.HyperConnect.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        // Destek sistemi tabloları
        DbSet<SupportTicket> SupportTickets { get; }
        DbSet<TicketNote> TicketNotes { get; }
        DbSet<TicketLog> TicketLogs { get; }
        DbSet<TicketAttachment> TicketAttachments { get; }
        DbSet<SupportTicketHistory> SupportTicketHistories { get; }

        DbSet<ManualOrder> ManualOrders { get; }
        DbSet<ManualOrderItem> ManualOrderItems { get; }

        // Bildirim sistemi
        DbSet<AppNotification> AppNotifications { get; }

        // Identity tabloları EKLENDİ
        DbSet<ApplicationUser> Users { get; }
        DbSet<IdentityUserRole<Guid>> UserRoles { get; }
        DbSet<IdentityRole<Guid>> Roles { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
