using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dekofar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        // Herkese açık
        [HttpGet("open")]
        public IActionResult OpenPing() => Ok("🌐 Bu endpoint herkese açık.");

        // Herkese açık
        [HttpGet("hello")]
        public IActionResult Hello() => Ok("Merhaba Dünya Recep");

        // Sadece geçerli JWT token’ı olanlar erişebilir
        [Authorize]
        [HttpGet("secure")]
        public IActionResult SecurePing() => Ok("🔐 Token doğrulandı, erişim sağlandı!");
    }
}
