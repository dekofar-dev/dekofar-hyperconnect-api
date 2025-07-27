using System;
using System.Collections.Generic;

namespace Dekofar.HyperConnect.Domain.Entities.Support
{
    /// <summary>
    /// Destek taleplerini temsil eden ana entity.
    /// </summary>
    public class SupportTicket
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Subject { get; set; } = null!;

        public string Description { get; set; } = null!;

        public SupportCategory Category { get; set; }

        public SupportPriority Priority { get; set; } = SupportPriority.Orta;

        public string? ShopifyOrderId { get; set; }

        public string? CustomerPhone { get; set; }

        public string? CustomerEmail { get; set; }

        public string? Tags { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? AssignedToUserId { get; set; }

        public SupportStatus Status { get; set; } = SupportStatus.Bekliyor;

        public ICollection<TicketNote> Notes { get; set; } = new List<TicketNote>();

        public ICollection<TicketLog> Logs { get; set; } = new List<TicketLog>();
    }

    public class TicketNote
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TicketId { get; set; }

        public string Message { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid CreatedBy { get; set; }

        public SupportTicket Ticket { get; set; } = null!;
    }

    public class TicketLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TicketId { get; set; }

        public string Action { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid CreatedBy { get; set; }

        public SupportTicket Ticket { get; set; } = null!;
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
        IptalEdildi = 4
    }
}
