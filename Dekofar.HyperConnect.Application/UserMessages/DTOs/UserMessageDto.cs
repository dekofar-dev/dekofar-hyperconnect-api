using System;

namespace Dekofar.HyperConnect.Application.UserMessages.DTOs
{
    public class UserMessageDto
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string? Text { get; set; }
        public string? AttachmentUrl { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
    }
}
