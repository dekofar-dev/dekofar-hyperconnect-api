using System;
using System.Collections.Generic;
using System.Linq;

namespace Dekofar.HyperConnect.Domain.Entities
{
    public class SupportCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public List<string> RelatedRoles { get; set; } = new();

        public ICollection<SupportTicket> Tickets { get; set; } = new List<SupportTicket>();
    }
}
