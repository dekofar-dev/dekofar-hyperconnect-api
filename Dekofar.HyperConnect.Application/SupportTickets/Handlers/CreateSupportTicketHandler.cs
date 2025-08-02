using Dekofar.HyperConnect.Application.Common.Interfaces;
using Dekofar.HyperConnect.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Application.SupportTickets.Handlers
{
    public class CreateSupportTicketHandler : IRequestHandler<CreateSupportTicketCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly IWebHostEnvironment _env;

        public CreateSupportTicketHandler(IApplicationDbContext context, ICurrentUserService currentUser, IWebHostEnvironment env)
        {
            _context = context;
            _currentUser = currentUser;
            _env = env;
        }

        public async Task<Guid> Handle(CreateSupportTicketCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.UserId == null)
                throw new UnauthorizedAccessException();

            var ticket = new SupportTicket
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                CreatedByUserId = _currentUser.UserId.Value,
                CategoryId = request.CategoryId,
                OrderId = request.OrderId,
                Priority = request.Priority,
                Status = SupportTicketStatus.Open,
                DueDate = request.DueDate,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            };

            if (request.File != null)
            {
                var uploads = Path.Combine(_env.ContentRootPath, "uploads");
                Directory.CreateDirectory(uploads);
                var fileName = Guid.NewGuid() + Path.GetExtension(request.File.FileName);
                var fullPath = Path.Combine(uploads, fileName);
                using var stream = new FileStream(fullPath, FileMode.Create);
                await request.File.CopyToAsync(stream, cancellationToken);
                ticket.FilePath = fullPath;
            }

            _context.SupportTickets.Add(ticket);
            await _context.SaveChangesAsync(cancellationToken);

            return ticket.Id;
        }
    }
}
