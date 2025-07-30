using System;

namespace Dekofar.HyperConnect.Domain.Entities.Support
{
    public class TicketAttachment
    {
        public long Id { get; set; }

        public int TicketId { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string? ContentType { get; set; }

        public byte[] Data { get; set; } = Array.Empty<byte>();

        public DateTime CreatedAt { get; set; }

        public Guid CreatedBy { get; set; }

        public SupportTicket Ticket { get; set; } = default!;
    }
}
