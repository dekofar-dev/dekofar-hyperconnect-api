using System;
using System.Collections.Generic;
namespace Dekofar.HyperConnect.Domain.Entities
{
    public class SupportCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<SupportCategoryRole> Roles { get; set; } = new List<SupportCategoryRole>();
        public ICollection<SupportTicket> Tickets { get; set; } = new List<SupportTicket>();
    }
}
