using Dekofar.HyperConnect.Domain.Common;
using Dekofar.HyperConnect.Domain.Enums;

namespace Dekofar.HyperConnect.Domain.Entities
{
    public class SupportTicket : BaseEntity
    {
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public SupportCategory Category { get; set; }  // ✅ Doğru enum kullanımı

        public Guid? AssignedToUserId { get; set; }
        public string? ShopifyOrderId { get; set; }

        public AppUser? AssignedToUser { get; set; }
        public List<TicketNote> Notes { get; set; } = new();
    }
}
