using Dekofar.HyperConnect.Application.DTOs.Support;
using Dekofar.HyperConnect.Application.Support.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Application.Interfaces
{

    public interface ISupportTicketService
    {
        Task<List<SupportTicketDto>> GetAllAsync();
        Task<SupportTicketDto?> GetByIdAsync(Guid id);
        Task<SupportTicketDto> CreateAsync(SupportTicketDto dto);
        Task AddNoteAsync(TicketNoteDto noteDto);
        Task AssignUserAsync(Guid ticketId, Guid userId);
    }
}
