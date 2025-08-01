using Dekofar.Domain.Entities;
using Dekofar.HyperConnect.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dekofar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")] // isteğe bağlı aktif edebilirsin
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users
                .Select(u => new
                {
                    u.Id,
                    u.FullName,
                    u.Email,
                    u.UserName
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost("change-role")]
        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRoleRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı");

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return BadRequest("Mevcut roller silinemedi");

            var roleResult = await _userManager.AddToRoleAsync(user, request.NewRole);
            if (!roleResult.Succeeded)
                return BadRequest("Yeni rol atanamadı");

            return Ok("Rol başarıyla güncellendi");
        }

        [HttpGet("assignable")]
        public async Task<IActionResult> GetAssignableUsers()
        {
            var assignableRoles = new[] { "Admin", "PERSONEL", "DEPO", "IADE", "MUSTERI_TEM" };


            var allUsers = await _userManager.Users.ToListAsync();
            var result = new List<object>();

            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any(r => assignableRoles.Contains(r)))
                {
                    result.Add(new
                    {
                        user.Id,
                        user.FullName,
                        user.Email,
                        Role = roles.FirstOrDefault()
                    });
                }
            }

            return Ok(result);
        }
    }
}
