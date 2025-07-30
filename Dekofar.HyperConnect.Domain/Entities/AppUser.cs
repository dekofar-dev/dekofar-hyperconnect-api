using Microsoft.AspNetCore.Identity;

namespace Dekofar.HyperConnect.Domain.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }
    }
}