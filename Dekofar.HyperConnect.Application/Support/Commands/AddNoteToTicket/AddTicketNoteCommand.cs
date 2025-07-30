using MediatR;

namespace Dekofar.HyperConnect.Application.Support.Commands.AddTicketNote
{
    public class AddTicketNoteCommand : IRequest<bool>
    {
        public int TicketId { get; set; }
        public string Message { get; set; } = null!;
    }
}
