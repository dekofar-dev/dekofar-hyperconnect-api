using System;

namespace Dekofar.HyperConnect.Domain.Entities
{
    public class SupportTicket
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public Guid CreatedByUserId { get; set; }
        public Guid? AssignedUserId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? OrderId { get; set; }
        public SupportTicketStatus Status { get; set; } = SupportTicketStatus.Open;
        public SupportTicketPriority Priority { get; set; } = SupportTicketPriority.Medium;
        public DateTime? DueDate { get; set; }
        public string? FilePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        public SupportCategory? Category { get; set; }
    }

    public enum SupportTicketStatus
    {
        Open = 0,
        InProgress = 1,
        Closed = 2
    }

    public enum SupportTicketPriority
    {
        Low = 0,
        Medium = 1,
        High = 2
    }
}
