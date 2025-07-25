using Dekofar.HyperConnect.Integrations.NetGsm.Interfaces;
using Dekofar.HyperConnect.Integrations.NetGsm.Models;
using Dekofar.HyperConnect.Integrations.NetGsm.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace dekofar_hyperconnect_api.Controllers.Integrations.NetGsm
{
    [ApiController]
    [Route("api/[controller]")]
    public class NetGsmController : ControllerBase
    {
        private readonly INetGsmSmsService _smsService;
        private readonly INetGsmCallService _callService;

        public NetGsmController(INetGsmSmsService smsService, INetGsmCallService callService)
        {
            _smsService = smsService;
            _callService = callService;
        }

        /// <summary>
        /// Gelen SMS kutusunu getirir (NetGSM inbox)
        /// </summary>
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




        /// <summary>
        /// Belirtilen tarih ve yöne göre çağrı kayıtlarını getirir (NetGSM voice report)
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
