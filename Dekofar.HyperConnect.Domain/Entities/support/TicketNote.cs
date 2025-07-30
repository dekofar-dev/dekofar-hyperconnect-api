using Dekofar.HyperConnect.Domain.Entities.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Domain.Entities.support
{
    public class TicketNote
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int TicketId { get; set; } // FK → SupportTicket.Id

        public string Message { get; set; } = null!;

        public Guid CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public SupportTicket Ticket { get; set; } = null!;
    }
}
