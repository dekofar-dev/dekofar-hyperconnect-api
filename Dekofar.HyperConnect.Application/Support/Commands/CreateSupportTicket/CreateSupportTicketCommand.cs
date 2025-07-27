using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Application.Support.Commands.CreateSupportTicket
{
    public class CreateSupportTicketCommand : IRequest<Guid>
    {
        public string Subject { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Category { get; set; }            // enum: SupportCategory
        public int? Priority { get; set; }           // enum: SupportPriority

        public string? ShopifyOrderId { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerEmail { get; set; }
        public string? Tags { get; set; }
    }
}
