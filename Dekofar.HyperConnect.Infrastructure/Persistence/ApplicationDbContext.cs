using Dekofar.HyperConnect.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dekofar.HyperConnect.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ✅ Support Ticket sistemi için DbSet'ler
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<TicketNote> TicketNotes { get; set; }

        // ✅ Fluent API veya seed işlemleri için override
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // İlişkilendirmeleri burada yapabilirsin (örnek):
            builder.Entity<SupportTicket>()
                .HasMany(t => t.Notes)
                .WithOne(n => n.Ticket)
                .HasForeignKey(n => n.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            // Gerekirse diğer ayarlar da buraya
        }
    }
}
