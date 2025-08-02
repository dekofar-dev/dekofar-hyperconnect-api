using Dekofar.Domain.Entities;
using Dekofar.HyperConnect.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<SupportTicket> SupportTickets { get; }
        DbSet<SupportCategory> SupportCategories { get; }
        DbSet<SupportCategoryRole> SupportCategoryRoles { get; }
        DbSet<ManualOrder> ManualOrders { get; }
        DbSet<ManualOrderItem> ManualOrderItems { get; }
        DbSet<OrderCommission> OrderCommissions { get; }
        DbSet<Discount> Discounts { get; }
        DbSet<ApplicationUser> Users { get; }
        DbSet<IdentityUserRole<Guid>> UserRoles { get; }
        DbSet<IdentityRole<Guid>> Roles { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
