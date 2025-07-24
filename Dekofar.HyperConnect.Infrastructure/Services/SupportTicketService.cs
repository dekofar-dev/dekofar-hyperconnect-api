using Dekofar.HyperConnect.Application.DTOs;
using Dekofar.HyperConnect.Application.Interfaces;
using Dekofar.HyperConnect.Domain.Entities;
using Dekofar.HyperConnect.Domain.Enums; // ✅ Enum buradan geliyor
using Dekofar.HyperConnect.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Dekofar.Infrastructure.Services;

public class SupportTicketService : ISupportTicketService
{
    private readonly ApplicationDbContext _context;

    public SupportTicketService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<SupportTicketDto>> GetAllAsync()
    {
        return await _context.SupportTickets
            .Include(x => x.Notes)
            .Select(ticket => new SupportTicketDto
            {
                Id = ticket.Id,
                Subject = ticket.Subject,
                Description = ticket.Description,
                Category = (int)ticket.Category,
                AssignedToUserId = ticket.AssignedToUserId,
                ShopifyOrderId = ticket.ShopifyOrderId,
                Notes = ticket.Notes.Select(note => new TicketNoteDto
                {
                    Id = note.Id,
                    TicketId = note.TicketId,
                    Note = note.Note,
                    CreatedByUserId = note.CreatedByUserId,
                    CreatedAt = note.CreatedAt
                }).ToList()
            }).ToListAsync();
    }

    public async Task<SupportTicketDto?> GetByIdAsync(Guid id)
    {
        var ticket = await _context.SupportTickets
            .Include(x => x.Notes)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (ticket == null) return null;

        return new SupportTicketDto
        {
            Id = ticket.Id,
            Subject = ticket.Subject,
            Description = ticket.Description,
            Category = (int)ticket.Category,
            AssignedToUserId = ticket.AssignedToUserId,
            ShopifyOrderId = ticket.ShopifyOrderId,
            Notes = ticket.Notes.Select(note => new TicketNoteDto
            {
                Id = note.Id,
                TicketId = note.TicketId,
                Note = note.Note,
                CreatedByUserId = note.CreatedByUserId,
                CreatedAt = note.CreatedAt
            }).ToList()
        };
    }

    public async Task<SupportTicketDto> CreateAsync(SupportTicketDto dto)
    {
        var ticket = new SupportTicket
        {
            Id = Guid.NewGuid(),
            Subject = dto.Subject,
            Description = dto.Description,
            Category = (SupportCategory)dto.Category,
            AssignedToUserId = dto.AssignedToUserId,
            ShopifyOrderId = dto.ShopifyOrderId,
            CreatedAt = DateTime.UtcNow
        };

        _context.SupportTickets.Add(ticket);
        await _context.SaveChangesAsync();

        dto.Id = ticket.Id;
        return dto;
    }

    public async Task AddNoteAsync(TicketNoteDto noteDto)
    {
        var note = new TicketNote
        {
            Id = Guid.NewGuid(),
            TicketId = noteDto.TicketId,
            Note = noteDto.Note,
            CreatedByUserId = noteDto.CreatedByUserId,
            CreatedAt = DateTime.UtcNow
        };

        _context.TicketNotes.Add(note);
        await _context.SaveChangesAsync();
    }

    public async Task AssignUserAsync(Guid ticketId, Guid userId)
    {
        var ticket = await _context.SupportTickets.FindAsync(ticketId);
        if (ticket == null) throw new Exception("Ticket not found");

        ticket.AssignedToUserId = userId;
        await _context.SaveChangesAsync();
    }
}
