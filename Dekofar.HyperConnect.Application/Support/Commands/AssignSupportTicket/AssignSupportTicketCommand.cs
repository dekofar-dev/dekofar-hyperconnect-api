using MediatR;

namespace Dekofar.HyperConnect.Application.Support.Commands.AssignSupportTicket
{
    public class AssignSupportTicketCommand : IRequest<bool>
    {
        public int TicketId { get; set; }
        public string? NewAssignedUserId { get; set; }
    }
}
