using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Application.Support.Commands.Queries.GetAllSupportTickets
{
    public class GetSupportTicketByIdQuery : IRequest<Application.DTOs.Support.SupportTicketDto>
    {
        public int TicketId { get; set; } // ✅ ID artık int

        public GetSupportTicketByIdQuery(int ticketId)
        {
            TicketId = ticketId;
        }
    }

}
