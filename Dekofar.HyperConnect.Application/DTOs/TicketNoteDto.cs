using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Application.DTOs
{
    public class TicketNoteDto
    {
        public Guid Id { get; set; }
        public Guid TicketId { get; set; }
        public string Note { get; set; } = string.Empty;
        public Guid CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
