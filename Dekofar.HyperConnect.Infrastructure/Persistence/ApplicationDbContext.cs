using Dekofar.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dekofar.HyperConnect.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        // Diğer DbSet<T>'ler buraya eklenecek
        // public DbSet<Product> Products { get; set; }
    }
}
