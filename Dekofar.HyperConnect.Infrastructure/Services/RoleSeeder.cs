using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Infrastructure.Services
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            string[] roles = new[] { "Admin", "User", "Support" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
        }
    }
}
