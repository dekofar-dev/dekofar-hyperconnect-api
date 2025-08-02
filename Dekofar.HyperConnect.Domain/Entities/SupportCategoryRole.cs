using System;

namespace Dekofar.HyperConnect.Domain.Entities
{
    public class SupportCategoryRole
    {
        public Guid Id { get; set; }
        public Guid SupportCategoryId { get; set; }
        public string RoleName { get; set; } = default!;

        public SupportCategory? Category { get; set; }
    }
}
