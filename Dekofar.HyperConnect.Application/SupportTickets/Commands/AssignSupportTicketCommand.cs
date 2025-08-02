using MediatR;
using System;

namespace Dekofar.HyperConnect.Application.SupportTickets.Commands
{
    public class AssignSupportTicketCommand : IRequest
    {
        public Guid TicketId { get; set; }
        public Guid AssignedUserId { get; set; }
    }
}
