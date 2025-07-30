using System;

namespace Dekofar.HyperConnect.Application.Support.DTOs
{
    public class TicketNoteDto
    {
        public Guid Id { get; set; }
        public int TicketId { get; set; }
        public string Message { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
