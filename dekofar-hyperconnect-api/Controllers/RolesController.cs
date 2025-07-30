using Dekofar.HyperConnect.Application.Users.Commands.AssignRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dekofar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public RolesController(RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")] // sadece adminler rol ekleyebilir
        public async Task<IActionResult> CreateRole([FromBody] RoleRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RoleName))
                return BadRequest("Rol adı boş olamaz.");

            var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
            if (roleExists)
                return Conflict("Bu rol zaten mevcut.");

            var result = await _roleManager.CreateAsync(new IdentityRole<Guid>(request.RoleName));
            if (result.Succeeded)
                return Ok($"'{request.RoleName}' rolü başarıyla oluşturuldu.");

            return BadRequest(result.Errors);
        }
    }
}
