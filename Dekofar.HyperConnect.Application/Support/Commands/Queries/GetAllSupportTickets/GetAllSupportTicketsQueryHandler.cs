using Dekofar.HyperConnect.Application.Common.Interfaces;
using Dekofar.HyperConnect.Application.DTOs.Support;
using Dekofar.HyperConnect.Application.Support.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Application.Support.Queries.GetAllSupportTickets
{
    public class GetAllSupportTicketsQueryHandler : IRequestHandler<GetAllSupportTicketsQuery, List<SupportTicketDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllSupportTicketsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SupportTicketDto>> Handle(GetAllSupportTicketsQuery request, CancellationToken cancellationToken)
        {
            var tickets = await _context.SupportTickets
                .Include(t => t.AssignedToUser)
                .Include(t => t.Notes)
                .Include(t => t.Logs)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(cancellationToken);

            var result = tickets.Select(t => new SupportTicketDto
            {
                Id = t.Id,
                TicketNumber = t.TicketNumber,
                Subject = t.Subject,
                Description = t.Description,
                Category = t.Category,
                Priority = t.Priority,
                Tags = t.Tags,
                ShopifyOrderId = t.ShopifyOrderId,
                CustomerEmail = t.CustomerEmail,
                CustomerPhone = t.CustomerPhone,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                CreatedBy = t.CreatedBy,

                // ✅ AssignedToUserId artık Guid? → string dönüşümüyle set ediliyor
                AssignedToUserId = t.AssignedToUserId,

                AssignedToUserName = t.AssignedToUser != null
                    ? t.AssignedToUser.FullName
                    : null,

                DueDate = t.DueDate,
                ResolvedAt = t.ResolvedAt,

                Notes = t.Notes.Select(n => new TicketNoteDto
                {
                    Id = n.Id,
                    TicketId = n.TicketId,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt,
                    CreatedBy = n.CreatedBy.ToString()
                }).ToList(),

                Logs = t.Logs.Select(l => new TicketLogDto
                {
                    Id = l.Id,
                    TicketId = l.TicketId,
                    Action = l.Action,
                    CreatedAt = l.CreatedAt,
                    CreatedBy = l.CreatedBy.ToString()
                }).ToList()

            }).ToList();

            return result;
        }
    }
}
