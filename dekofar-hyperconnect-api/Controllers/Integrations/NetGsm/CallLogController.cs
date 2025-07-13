using Dekofar.HyperConnect.Integrations.NetGsm.Interfaces;
using Dekofar.HyperConnect.Integrations.NetGsm.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dekofar.HyperConnect.API.Controllers.Integrations
{
    [ApiController]
    [Route("api/[controller]")]
    public class NetGsmController : ControllerBase
    {
        private readonly INetGsmCallService _callService;

        public NetGsmController(INetGsmCallService callService)
        {
            _callService = callService;
        }

        /// <summary>
        /// NetGSM çağrı kayıtlarını getirir
        /// </summary>
        [HttpPost("call-logs")]
        public async Task<IActionResult> GetCallLogs([FromBody] CallLogRequest request)
        {
            try
            {
                var result = await _callService.GetCallLogsAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
