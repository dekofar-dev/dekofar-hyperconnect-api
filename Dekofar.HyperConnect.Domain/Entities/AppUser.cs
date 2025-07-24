using Microsoft.AspNetCore.Identity;

namespace Dekofar.HyperConnect.Domain.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;

        // Eğer kullanıcıya ait ek bilgiler olacaksa buraya ekleyebilirsin
        // Örnek: public string Department { get; set; }
    }
}