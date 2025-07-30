using Dekofar.HyperConnect.Application.Support.DTOs;
using Dekofar.HyperConnect.Domain.Entities.Support;

namespace Dekofar.HyperConnect.Application.DTOs.Support
{
    public class SupportTicketDto
    {
        public int Id { get; set; }
        public string TicketNumber { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string? Description { get; set; }
        public SupportCategory Category { get; set; }
        public SupportPriority Priority { get; set; }
        public string? Tags { get; set; }
        public string? ShopifyOrderId { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerEmail { get; set; }

        // 🔄 Güncellendi: string → Guid?
        public Guid? AssignedToUserId { get; set; }

        // 👤 Atanan kullanıcının adı (opsiyonel görüntüleme için)
        public string? AssignedToUserName { get; set; }

        public SupportStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        // 🔄 string yerine Guid: güvenli ilişkilendirme
        public Guid CreatedBy { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime? ResolvedAt { get; set; }

        // 📝 Notlar
        public List<TicketNoteDto> Notes { get; set; } = new();

        // 📜 Geçmiş kayıtları
        public List<TicketLogDto> Logs { get; set; } = new();
    }
}
