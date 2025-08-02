using System;

namespace Dekofar.HyperConnect.Domain.Entities
{
    public class RolePermission
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; } = default!;
        public Guid PermissionId { get; set; }

        public Permission? Permission { get; set; }
    }
}
