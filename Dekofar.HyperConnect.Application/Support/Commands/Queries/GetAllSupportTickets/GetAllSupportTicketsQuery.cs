using Dekofar.HyperConnect.Application.DTOs.Support;
using MediatR;
using System.Collections.Generic;

namespace Dekofar.HyperConnect.Application.Support.Queries.GetAllSupportTickets
{
    public class GetAllSupportTicketsQuery : IRequest<List<SupportTicketDto>>
    {
    }
}
