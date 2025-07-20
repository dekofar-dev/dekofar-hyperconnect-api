using Dekofar.HyperConnect.Integrations.NetGsm.Models;
using Dekofar.HyperConnect.Integrations.NetGsm.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace dekofar_hyperconnect_api.Controllers.Integrations.NetGsm
{
    [ApiController]
    [Route("api/[controller]")]
    public class NetGsmController : ControllerBase
    {
        private readonly INetGsmSmsService _smsService;

        public NetGsmController(INetGsmSmsService smsService)
        {
            _smsService = smsService;
        }

        [HttpPost("sms-inbox")]
        public async Task<IActionResult> GetSmsInbox([FromBody] SmsInboxRequest request)
        {
            try
            {
                var result = await _smsService.GetInboxMessagesAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
