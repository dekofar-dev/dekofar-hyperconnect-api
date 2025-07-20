using Dekofar.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Dekofar.HyperConnect.Domain.DTOs;

namespace Dekofar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN")] // Sadece admin erişsin
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.Select(u => new
            {
                u.Id,
                u.FullName,
                u.Email,
                u.UserName
            }).ToList();

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

    }
}
