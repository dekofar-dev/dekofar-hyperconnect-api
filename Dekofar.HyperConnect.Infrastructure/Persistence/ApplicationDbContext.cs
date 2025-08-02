using Dekofar.Domain.Entities;
using Dekofar.HyperConnect.Application.Common.Interfaces;
using Dekofar.HyperConnect.Domain.Entities;
using Dekofar.HyperConnect.Domain.Entities.Orders;
using Dekofar.HyperConnect.Domain.Entities.support;
using Dekofar.HyperConnect.Domain.Entities.Support;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dekofar.HyperConnect.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
        public DbSet<TicketNote> TicketNotes => Set<TicketNote>();
        public DbSet<TicketLog> TicketLogs => Set<TicketLog>();
        public DbSet<TicketAttachment> TicketAttachments => Set<TicketAttachment>();
        public DbSet<SupportTicketHistory> SupportTicketHistories => Set<SupportTicketHistory>();
        public DbSet<AppNotification> AppNotifications => Set<AppNotification>();
        public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
        public DbSet<IdentityUserRole<Guid>> UserRoles => Set<IdentityUserRole<Guid>>();
        public DbSet<IdentityRole<Guid>> Roles => Set<IdentityRole<Guid>>();

        public DbSet<Tag> Tags { get; set; }
        public DbSet<OrderTag> OrderTags => Set<OrderTag>();
        public DbSet<ManualOrder> ManualOrders => Set<ManualOrder>();
        public DbSet<ManualOrderItem> ManualOrderItems => Set<ManualOrderItem>();


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
                entity.Property(e => e.Priority).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.TicketNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Tags).HasMaxLength(255);
                entity.Property(e => e.CustomerPhone).HasMaxLength(50);
                entity.Property(e => e.CustomerEmail).HasMaxLength(100);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.CreatedBy).IsRequired();
                entity.Property(e => e.AssignedToUserId);
                entity.Property(e => e.DueDate);
                entity.Property(e => e.ResolvedAt);

                // 🔧 İlişki düzeltmesi (string FK → Guid PK)
                entity.HasOne(e => e.AssignedToUser)
                      .WithMany()
                      .HasForeignKey(e => e.AssignedToUserId)
                      .OnDelete(DeleteBehavior.Restrict);

            });




            // TicketNote
            builder.Entity<TicketNote>(entity =>
            {
                entity.ToTable("TicketNotes");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.TicketId).IsRequired();
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.CreatedBy).IsRequired();

                entity.HasOne(n => n.Ticket)
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
                entity.Property(e => e.CreatedBy).IsRequired();

                entity.HasOne(l => l.Ticket)
                      .WithMany(t => t.Logs)
                      .HasForeignKey(l => l.TicketId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // TicketAttachment
            builder.Entity<TicketAttachment>(entity =>
            {
                entity.ToTable("TicketAttachments");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.TicketId).IsRequired();
                entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.ContentType).HasMaxLength(100);
                entity.Property(e => e.Data).IsRequired(); // byte[] içerik
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.CreatedBy).IsRequired();

                entity.HasOne(a => a.Ticket)
                      .WithMany(t => t.Attachments)
                      .HasForeignKey(a => a.TicketId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // SupportTicketHistory
            builder.Entity<SupportTicketHistory>(entity =>
            {
                entity.ToTable("SupportTicketHistories");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.TicketId).IsRequired();
                entity.Property(e => e.FieldChanged).IsRequired().HasMaxLength(100);
                entity.Property(e => e.OldValue).HasMaxLength(255);
                entity.Property(e => e.NewValue).HasMaxLength(255);
                entity.Property(e => e.ChangedAt).IsRequired();
                entity.Property(e => e.ChangedBy).IsRequired();

                entity.HasOne(h => h.Ticket)
                      .WithMany(t => t.History)
                      .HasForeignKey(h => h.TicketId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ManualOrder
            builder.Entity<ManualOrder>(entity =>
            {
                entity.ToTable("ManualOrders");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CustomerSurname).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(500);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.District).HasMaxLength(100);
                entity.Property(e => e.PaymentType).HasMaxLength(50);
                entity.Property(e => e.OrderNote).HasMaxLength(500);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.DiscountName).HasMaxLength(100);
                entity.Property(e => e.DiscountType).HasMaxLength(50);
                entity.Property(e => e.DiscountValue).HasColumnType("decimal(18,2)");
                entity.Property(e => e.BonusAmount).HasColumnType("decimal(18,2)");

                entity.HasMany(e => e.Items)
                      .WithOne(i => i.ManualOrder)
                      .HasForeignKey(i => i.ManualOrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ManualOrderItem>(entity =>
            {
                entity.ToTable("ManualOrderItems");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.ProductId).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Total).HasColumnType("decimal(18,2)");
            });

            // AppNotification
            builder.Entity<AppNotification>(entity =>
            {
                entity.ToTable("AppNotifications");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Message).IsRequired().HasMaxLength(500);
                entity.Property(e => e.IsRead).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });
        }
    }
}
