using Dekofar.HyperConnect.Application.Common.Interfaces;
using Dekofar.HyperConnect.Domain.Entities;
using Dekofar.HyperConnect.Domain.Entities.Support;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dekofar.HyperConnect.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Destek sistemi tabloları
        public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
        public DbSet<TicketNote> TicketNotes => Set<TicketNote>();
        public DbSet<TicketLog> TicketLogs => Set<TicketLog>();

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await base.SaveChangesAsync(cancellationToken);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // SupportTicket
            builder.Entity<SupportTicket>(entity =>
            {
                entity.ToTable("SupportTickets");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Subject).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Category).IsRequired();
                entity.Property(e => e.Priority);
                entity.Property(e => e.Tags);
                entity.Property(e => e.CustomerPhone).HasMaxLength(50);
                entity.Property(e => e.CustomerEmail).HasMaxLength(100);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.CreatedBy).HasMaxLength(100);
                entity.Property(e => e.AssignedToUserId).HasMaxLength(100);
                entity.Property(e => e.Status).IsRequired();
            });

            // TicketNote
            builder.Entity<TicketNote>(entity =>
            {
                entity.ToTable("TicketNotes");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.TicketId).IsRequired();
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.HasOne<SupportTicket>()
                      .WithMany(t => t.Notes)
                      .HasForeignKey(n => n.TicketId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // TicketLog
            builder.Entity<TicketLog>(entity =>
            {
                entity.ToTable("TicketLogs");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.TicketId).IsRequired();
                entity.Property(e => e.Action).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.HasOne<SupportTicket>()
                      .WithMany(t => t.Logs)
                      .HasForeignKey(l => l.TicketId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
