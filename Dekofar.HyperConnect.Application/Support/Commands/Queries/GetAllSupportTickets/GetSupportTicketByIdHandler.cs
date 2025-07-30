using Dekofar.HyperConnect.Application.Common.Interfaces;
using Dekofar.HyperConnect.Application.DTOs.Support;
using Dekofar.HyperConnect.Application.Support.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Application.Support.Commands.Queries.GetAllSupportTickets
{
    public class GetSupportTicketByIdHandler : IRequestHandler<GetSupportTicketByIdQuery, SupportTicketDto?>
    {
        private readonly IApplicationDbContext _context;

        public GetSupportTicketByIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SupportTicketDto?> Handle(GetSupportTicketByIdQuery request, CancellationToken cancellationToken)
        {
            var ticket = await _context.SupportTickets
                .Include(x => x.AssignedToUser)
                .Include(x => x.Notes)
                .Include(x => x.Logs)
                .FirstOrDefaultAsync(x => x.Id == request.TicketId, cancellationToken);

            if (ticket == null)
                return null;
            return new SupportTicketDto
            {
                Id = ticket.Id,
                TicketNumber = ticket.TicketNumber,
                Subject = ticket.Subject,
                Description = ticket.Description,
                Category = ticket.Category,
                Priority = ticket.Priority,
                Tags = ticket.Tags,
                CustomerPhone = ticket.CustomerPhone,
                CustomerEmail = ticket.CustomerEmail,

                AssignedToUserId = ticket.AssignedToUserId, // ✅ düzeltildi
                AssignedToUserName = ticket.AssignedToUser?.FullName,

                CreatedAt = ticket.CreatedAt,
                CreatedBy = ticket.CreatedBy, // Guid olarak doğru
                ShopifyOrderId = ticket.ShopifyOrderId,
                Status = ticket.Status,
                DueDate = ticket.DueDate,
                ResolvedAt = ticket.ResolvedAt,

                Notes = ticket.Notes?.Select(n => new TicketNoteDto
                {
                    Id = n.Id,
                    TicketId = n.TicketId,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt,
                    CreatedBy = n.CreatedBy.ToString()
                }).ToList() ?? new List<TicketNoteDto>(),

                Logs = ticket.Logs?.Select(l => new TicketLogDto
                {
                    Id = l.Id,
                    TicketId = l.TicketId,
                    Action = l.Action,
                    CreatedAt = l.CreatedAt,
                    CreatedBy = l.CreatedBy.ToString()
                }).ToList() ?? new List<TicketLogDto>()
            };

        }
    }
}
