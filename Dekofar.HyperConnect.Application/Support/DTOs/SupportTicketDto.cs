using System;
using System.Collections.Generic;
using Dekofar.HyperConnect.Domain.Entities.Support;
using Dekofar.HyperConnect.Application.Support.DTOs;

namespace Dekofar.HyperConnect.Application.DTOs.Support
{
    public class SupportTicketDto
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public SupportCategory Category { get; set; }
        public SupportPriority Priority { get; set; }
        public string? Tags { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerEmail { get; set; }
        public Guid? ShopifyOrderId { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public string? AssignedToUserName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public SupportStatus Status { get; set; }

        public List<TicketNoteDto> Notes { get; set; } = new();
        public List<TicketLogDto> Logs { get; set; } = new();
    }
}
