using Dekofar.HyperConnect.Domain.Entities.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Domain.Entities.support
{
    public class TicketLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TicketId { get; set; }

        public Guid CreatedBy { get; set; }

        public string Action { get; set; }  // örn: "Durum değiştirildi", "Not eklendi", "Kullanıcı atandı"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property (isteğe bağlı)
        public SupportTicket Ticket { get; set; }
    }
}
