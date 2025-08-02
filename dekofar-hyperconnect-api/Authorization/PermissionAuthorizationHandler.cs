using System;
using System.Linq;
using System.Threading.Tasks;
using Dekofar.Domain.Entities;
using Dekofar.HyperConnect.API.Authorization;
using Dekofar.HyperConnect.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dekofar.HyperConnect.API.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;

        public PermissionAuthorizationHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userId = context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return;
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return;
            }

            var roles = await _userManager.GetRolesAsync(user);
            var permission = await _context.Permissions
                .Include(p => p.RolePermissions)
                .FirstOrDefaultAsync(p => p.Name == requirement.Permission);

            if (permission == null)
                return;

            var hasPermission = permission.RolePermissions != null &&
                                permission.RolePermissions.Any(rp => roles.Contains(rp.RoleName));

            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }
}
