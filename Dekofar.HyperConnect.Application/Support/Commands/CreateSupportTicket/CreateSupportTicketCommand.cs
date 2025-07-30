using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace Dekofar.HyperConnect.Application.Support.Commands.CreateSupportTicket
{
    public class CreateSupportTicketCommand : IRequest<int>
    {
        public string Subject { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Category { get; set; }            // enum: SupportCategory
        public int? Priority { get; set; }           // enum: SupportPriority
        public string? ShopifyOrderId { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerEmail { get; set; }
        public string? Tags { get; set; }
        public DateTime? DueDate { get; set; }       // Opsiyonel SLA tarihi
        public IFormFileCollection? Attachments { get; set; } // Dosya ekleri
        public string? AssignedToUserId { get; set; } // bu satırı ekle

    }
}
