using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dekofar.HyperConnect.Application.Users.Commands;
using Dekofar.HyperConnect.Application.Users.DTOs;
using Dekofar.HyperConnect.Application.Users.Queries;
using Dekofar.HyperConnect.Application.Interfaces;
using Dekofar.API.Authorization;
using Dekofar.HyperConnect.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dekofar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Kullanıcı yönetimi ile ilgili işlemleri sağlayan controller
    public class UsersController : ControllerBase
    {
        // MediatR aracısı
        private readonly IMediator _mediator;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        // MediatR bağımlılığını alan kurucu
        public UsersController(IMediator mediator, IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userService = userService;
            _userManager = userManager;
        }

        // Sistemdeki tüm kullanıcıları döner
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var users = await _mediator.Send(new GetAllUsersWithRolesQuery());
            return Ok(users);
        }

        // Kullanıcıya rol ataması yapar
        [HttpPost("{id:guid}/roles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRoles(Guid id, [FromBody] List<string> roles)
        {
            var result = await _mediator.Send(new AssignRolesToUserCommand { UserId = id, Roles = roles });
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        // Kullanıcının profil resmini yükler
        [HttpPost("{id:guid}/avatar")]
        [Authorize]
        public async Task<IActionResult> UploadAvatar(Guid id, [FromForm] IFormFile file)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null || currentUserId != id.ToString())
            {
                return Forbid();
            }

            var url = await _mediator.Send(new UploadProfileImageCommand { UserId = id, File = file });
            return Ok(new { avatarUrl = url });
        }

        [HttpGet("me/stats")]
        [Authorize]
        public async Task<IActionResult> GetMyProfileWithStats()
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();
            var profile = await _userService.GetProfileWithStatsAsync(userId.Value);
            return Ok(profile);
        }

        [HttpGet("me/summary")]
        [Authorize]
        public async Task<ActionResult<ProfileSummaryDto>> GetProfileSummary()
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();
            var summary = await _userService.GetProfileSummaryAsync(userId.Value);
            if (summary == null) return NotFound();
            return Ok(summary);
        }

        [HttpGet("{id:guid}/sales-summary")]
        [Authorize]
        public async Task<ActionResult<SalesSummaryDto>> GetSalesSummary(Guid id)
        {
            var summary = await _userService.GetSalesSummaryAsync(id);
            if (summary == null) return NotFound();
            return Ok(summary);
        }

        public class PinRequest
        {
            public string Pin { get; set; } = string.Empty;
        }

        [HttpPost("set-pin")]
        [Authorize]
        public async Task<IActionResult> SetPin([FromBody] PinRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Pin) || request.Pin.Length != 4 || !request.Pin.All(char.IsDigit))
                return BadRequest("PIN must be exactly 4 digits.");

            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId.Value.ToString());
            if (user == null) return NotFound();

            user.HashedPin = _userManager.PasswordHasher.HashPassword(user, request.Pin);
            user.PinLastUpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return Ok();
        }

        [HttpPost("verify-pin")]
        [Authorize]
        public async Task<IActionResult> VerifyPin([FromBody] PinRequest request)
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId.Value.ToString());
            if (user == null || string.IsNullOrEmpty(user.HashedPin))
                return Unauthorized();

            var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.HashedPin, request.Pin);
            if (result == PasswordVerificationResult.Success)
            {
                return Ok();
            }

            return Unauthorized();
        }

        [HttpPost("{id:guid}/reset-pin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetPin(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound();
            user.HashedPin = null;
            user.PinLastUpdatedAt = null;
            await _userManager.UpdateAsync(user);
            return Ok();
        }
    }
}
