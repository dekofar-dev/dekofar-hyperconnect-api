using Dekofar.HyperConnect.Domain.Entities.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Domain.Entities.support
{
    public class SupportTicketHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int TicketId { get; set; }
        public string FieldChanged { get; set; } = string.Empty;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
        public Guid ChangedBy { get; set; }

        public SupportTicket Ticket { get; set; } = null!;
    }

}