using Dekofar.Domain.Entities;
using Dekofar.HyperConnect.Domain.Entities.support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dekofar.HyperConnect.Domain.Entities.Support
{
    /// <summary>
    /// Destek taleplerini temsil eden ana entity.
    /// </summary>
    public class SupportTicket
    {
        public int Id { get; set; }

        public string TicketNumber { get; set; } = string.Empty;

        public string Subject { get; set; } = null!;
        public string Description { get; set; } = null!;

        public SupportCategory Category { get; set; }
        public SupportPriority Priority { get; set; } = SupportPriority.Orta;

        public string ShopifyOrderId { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }
        public string? CustomerEmail { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ResolvedAt { get; set; }

        public string? Tags { get; set; }

        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid? AssignedToUserId { get; set; }

        [ForeignKey(nameof(AssignedToUserId))] // ✅ Eklenen kısım: ilişkiyi net belirtiyoruz
        public ApplicationUser? AssignedToUser { get; set; }

        public SupportStatus Status { get; set; } = SupportStatus.Bekliyor;

        public ICollection<TicketNote> Notes { get; set; } = new List<TicketNote>();
        public ICollection<TicketLog> Logs { get; set; } = new List<TicketLog>();
        public ICollection<SupportTicketHistory> History { get; set; } = new List<SupportTicketHistory>();
        public ICollection<TicketAttachment> Attachments { get; set; } = new List<TicketAttachment>();
    }

    public enum SupportCategory
    {
        Iade = 0,
        Degisim = 1,
        EksikUrun = 2,
        KargoSorunu = 3,
        OdemeSorunu = 4,
        GenelBilgi = 5,
        Diger = 6,
        Garanti = 7
    }

    public enum SupportPriority
    {
        Yuksek = 1,
        Orta = 2,
        Dusuk = 3
    }

    public enum SupportStatus
    {
        Bekliyor = 0,
        Atandi = 1,
        DevamEdiyor = 2,
        Cozuldu = 3,
        IptalEdildi = 4,
        Kapandi = 5

    }
}
