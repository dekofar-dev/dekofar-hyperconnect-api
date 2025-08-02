using System;
using System.Threading.Tasks;
using Dekofar.Domain.Entities;
using Dekofar.HyperConnect.Domain.Entities;
using Dekofar.HyperConnect.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dekofar.HyperConnect.Infrastructure.Services
{
    public static class SeedData
    {
        public static async Task SeedDefaultsAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            string[] roles = new[] { "Admin", "Support", "Warehouse", "Returns", "Finance" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            var adminEmail = "admin@dekofar.com";
            var adminPassword = "AdminRecep123*";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    MembershipDate = DateTime.UtcNow
                };
                var createUserResult = await userManager.CreateAsync(adminUser, adminPassword);
                if (!createUserResult.Succeeded)
                {
                    return;
                }
            }

            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            var generalCategory = await context.SupportCategories.FirstOrDefaultAsync();
            if (generalCategory == null)
            {
                generalCategory = new SupportCategory
                {
                    Id = Guid.NewGuid(),
                    Name = "General",
                    Description = "General support",
                    CreatedAt = DateTime.UtcNow
                };
                context.SupportCategories.Add(generalCategory);
                await context.SaveChangesAsync();
            }

            if (!await context.SupportTickets.AnyAsync())
            {
                var ticket = new SupportTicket
                {
                    Id = Guid.NewGuid(),
                    Title = "Test ticket",
                    Description = "This is a seeded support ticket",
                    CreatedByUserId = adminUser.Id,
                    CategoryId = generalCategory.Id,
                    Status = SupportTicketStatus.Open,
                    Priority = SupportTicketPriority.Medium,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                };

                context.SupportTickets.Add(ticket);
                await context.SaveChangesAsync();
            }

            // Seed default permissions used by policy-based authorization
            var defaultPermissions = new[]
            {
                new Permission { Id = Guid.NewGuid(), Name = "CanAssignTicket", Description = "Assign support tickets" },
                new Permission { Id = Guid.NewGuid(), Name = "CanManageDiscounts", Description = "Manage discounts" },
                new Permission { Id = Guid.NewGuid(), Name = "CanEditDueDate", Description = "Edit ticket due dates" }
            };

            foreach (var perm in defaultPermissions)
            {
                if (!await context.Permissions.AnyAsync(p => p.Name == perm.Name))
                {
                    context.Permissions.Add(perm);
                }
            }
            await context.SaveChangesAsync();

            var adminRoleName = "Admin";
            var adminPermissions = await context.Permissions.ToListAsync();

            // Ensure the Admin role has all default permissions
            foreach (var perm in adminPermissions)
            {
                if (!await context.RolePermissions.AnyAsync(rp => rp.RoleName == adminRoleName && rp.PermissionId == perm.Id))
                {
                    context.RolePermissions.Add(new RolePermission
                    {
                        Id = Guid.NewGuid(),
                        RoleName = adminRoleName,
                        PermissionId = perm.Id
                    });
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
