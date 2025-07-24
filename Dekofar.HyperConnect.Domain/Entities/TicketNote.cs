using Dekofar.HyperConnect.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Domain.Entities
{
    public class TicketNote : BaseEntity
    {
        public Guid TicketId { get; set; }
        public string Note { get; set; } = string.Empty;
        public Guid CreatedByUserId { get; set; }

        public SupportTicket? Ticket { get; set; }
        public AppUser? CreatedByUser { get; set; }
    }
}
