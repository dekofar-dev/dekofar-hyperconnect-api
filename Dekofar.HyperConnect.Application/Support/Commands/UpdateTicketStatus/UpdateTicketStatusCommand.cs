using MediatR;

namespace Dekofar.HyperConnect.Application.Support.Commands.UpdateTicketStatus
{
    public class UpdateTicketStatusCommand : IRequest<bool>
    {
        public int TicketId { get; set; }
        public int NewStatus { get; set; } // enum: SupportStatus
    }
}
