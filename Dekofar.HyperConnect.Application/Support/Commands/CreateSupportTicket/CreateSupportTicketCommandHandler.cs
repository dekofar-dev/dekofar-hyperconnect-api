using Dekofar.HyperConnect.Application.Common.Interfaces;
using Dekofar.HyperConnect.Domain.Entities.support;
using Dekofar.HyperConnect.Domain.Entities.Support;
using Dekofar.HyperConnect.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Application.Support.Commands.CreateSupportTicket
{
    public class CreateSupportTicketCommandHandler : IRequestHandler<CreateSupportTicketCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        private readonly Dictionary<SupportCategory, List<string>> _categoryRoleMap = new()
        {
            { SupportCategory.Iade, new List<string> { "IADE" } },
            { SupportCategory.Degisim, new List<string> { "IADE" } },
            { SupportCategory.KargoSorunu, new List<string> { "DEPO" } },
            { SupportCategory.EksikUrun, new List<string> { "DEPO", "IADE" } },
            { SupportCategory.OdemeSorunu, new List<string> { "MUHASEBE" } },
            { SupportCategory.GenelBilgi, new List<string> { "DESTEK" } },
            { SupportCategory.Garanti, new List<string> { "IADE" } },
            { SupportCategory.Diger, new List<string> { "DESTEK" } }
        };

        public CreateSupportTicketCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<int> Handle(CreateSupportTicketCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.UserId == null)
                throw new UnauthorizedAccessException("Kullanıcı doğrulanamadı.");

            var userId = _currentUser.UserId.Value;
            var categoryEnum = (SupportCategory)request.Category;

            var prefix = categoryEnum.GetCode();
            var count = await _context.SupportTickets.CountAsync(x => x.Category == categoryEnum, cancellationToken);
            var ticketNumber = $"{prefix}-{(count + 1):D3}";

            var ticket = new SupportTicket
            {
                TicketNumber = ticketNumber,
                Subject = request.Subject,
                Description = request.Description,
                Category = categoryEnum,
                Priority = request.Priority.HasValue ? (SupportPriority)request.Priority.Value : SupportPriority.Orta,
                Tags = request.Tags,
                ShopifyOrderId = request.ShopifyOrderId ?? string.Empty,
                CustomerPhone = request.CustomerPhone,
                CustomerEmail = request.CustomerEmail,
                Status = SupportStatus.Bekliyor,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId,
                DueDate = request.DueDate ?? DateTime.UtcNow.AddDays(3),
                ResolvedAt = null,
                Attachments = new List<TicketAttachment>()
            };

            // ✅ Otomatik atama
            if (string.IsNullOrWhiteSpace(request.AssignedToUserId))
            {
                if (_categoryRoleMap.TryGetValue(categoryEnum, out var roleNames))
                {
                    var usersInRoles = await _context.Users
                        .Where(u => _context.UserRoles
                            .Any(ur => ur.UserId == u.Id && _context.Roles
                                .Any(r => r.Id == ur.RoleId && roleNames.Contains(r.Name))))
                        .ToListAsync(cancellationToken);

                    if (usersInRoles.Any())
                    {
                        var userTickets = await _context.SupportTickets
                            .Where(t => t.Status != SupportStatus.Kapandi)
                            .GroupBy(t => t.AssignedToUserId)
                            .Select(g => new { UserId = g.Key, Count = g.Count() })
                            .ToListAsync(cancellationToken);

                        var userWithLeastTickets = usersInRoles
                            .Select(u => new
                            {
                                User = u,
                                TicketCount = userTickets.FirstOrDefault(x => x.UserId == u.Id)?.Count ?? 0
                            })
                            .OrderBy(x => x.TicketCount)
                            .FirstOrDefault();

                        if (userWithLeastTickets != null)
                        {
                            ticket.AssignedToUserId = userWithLeastTickets.User.Id;
                        }
                    }
                }

                // Hiç kimse atanamadıysa kendisine ata
                if (ticket.AssignedToUserId == null)
                    ticket.AssignedToUserId = userId;
            }
            else
            {
                if (Guid.TryParse(request.AssignedToUserId, out var parsedGuid))
                    ticket.AssignedToUserId = parsedGuid;
            }

            // Dosya ekleri
            if (request.Attachments != null && request.Attachments.Any())
            {
                foreach (var file in request.Attachments)
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms, cancellationToken);

                    ticket.Attachments.Add(new TicketAttachment
                    {
                        FileName = file.FileName,
                        ContentType = file.ContentType,
                        Data = ms.ToArray(),
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId
                    });
                }
            }

            await _context.SupportTickets.AddAsync(ticket, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _context.TicketLogs.AddAsync(new TicketLog
            {
                TicketId = ticket.Id,
                Action = "Destek talebi oluşturuldu.",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId
            }, cancellationToken);

            await _context.SupportTicketHistories.AddAsync(new SupportTicketHistory
            {
                TicketId = ticket.Id,
                FieldChanged = "Status",
                OldValue = null,
                NewValue = ticket.Status.ToString(),
                ChangedAt = DateTime.UtcNow,
                ChangedBy = userId
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return ticket.Id;
        }
    }
}
