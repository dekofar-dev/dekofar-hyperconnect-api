using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Application.DTOs
{
    public class SupportTicketDto
    {
        public Guid Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Category { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public string? ShopifyOrderId { get; set; }
        public List<TicketNoteDto> Notes { get; set; } = new();
    }
}
