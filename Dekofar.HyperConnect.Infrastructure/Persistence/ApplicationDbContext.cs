using Dekofar.Domain.Entities;
using Dekofar.HyperConnect.Application.Common.Interfaces;
using Dekofar.HyperConnect.Domain.Entities;
using Dekofar.HyperConnect.Domain.Entities.Orders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Dekofar.HyperConnect.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
        public DbSet<SupportCategory> SupportCategories => Set<SupportCategory>();
        public DbSet<SupportCategoryRole> SupportCategoryRoles => Set<SupportCategoryRole>();
        public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
        public DbSet<IdentityUserRole<Guid>> UserRoles => Set<IdentityUserRole<Guid>>();
        public DbSet<IdentityRole<Guid>> Roles => Set<IdentityRole<Guid>>();
        public DbSet<Tag> Tags { get; set; }
        public DbSet<OrderTag> OrderTags => Set<OrderTag>();
        public DbSet<ManualOrder> ManualOrders => Set<ManualOrder>();
        public DbSet<ManualOrderItem> ManualOrderItems => Set<ManualOrderItem>();
        public DbSet<Discount> Discounts => Set<Discount>();

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await base.SaveChangesAsync(cancellationToken);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SupportCategory>(entity =>
            {
                entity.ToTable("SupportCategories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(250);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.HasMany(e => e.Roles)
                      .WithOne(r => r.Category)
                      .HasForeignKey(r => r.SupportCategoryId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<SupportCategoryRole>(entity =>
            {
                entity.ToTable("SupportCategoryRoles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RoleName).IsRequired().HasMaxLength(100);
            });

            builder.Entity<SupportTicket>(entity =>
            {
                entity.ToTable("SupportTickets");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.FilePath).HasMaxLength(500);
                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Tickets)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

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
            });

            builder.Entity<Discount>(entity =>
            {
                entity.ToTable("Discounts");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.Value).HasColumnType("decimal(18,2)");
                entity.Property(e => e.IsActive).IsRequired();
                entity.Property(e => e.CreatedByUserId).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });
        }
    }
}
