using Dekofar.Domain.Entities;
using Dekofar.HyperConnect.Application.Auth;
using Dekofar.HyperConnect.Application.Interfaces;
using Dekofar.HyperConnect.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Dekofar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request,
            [FromServices] UserManager<AppUser> userManager,
            [FromServices] RoleManager<IdentityRole<Guid>> roleManager)
        {
            var user = new AppUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // ✅ Admin rolünü ata
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));

            await userManager.AddToRoleAsync(user, "Admin");

            return Ok(new { message = "Kullanıcı başarıyla oluşturuldu." });
        }

        public class RegisterRequest
        {
            public string FullName { get; set; } = "";
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized("Kullanıcı bulunamadı.");

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!signInResult.Succeeded)
                return Unauthorized("Şifre hatalı.");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "PERSONEL";
            var token = _tokenService.GenerateToken(user.Id.ToString(), user.Email, role);

            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.FullName,
                    user.Email,
                    Role = role
                }
            });
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                user.Id,
                user.Email,
                user.FullName,
                Role = roles.FirstOrDefault()
            });
        }
    }
}
