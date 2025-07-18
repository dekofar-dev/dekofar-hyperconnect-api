using Dekofar.HyperConnect.Integrations.NetGsm.Models;
using Dekofar.HyperConnect.Integrations.NetGsm.Services;
using Microsoft.AspNetCore.Mvc;

namespace dekofar_hyperconnect_api.Controllers.Integrations.NetGsm
{
    [ApiController]
    [Route("api/netgsm/sms")]
    public class NetGsmSmsController : ControllerBase
    {
        private readonly NetGsmSmsService _smsService;
        private readonly ILogger<NetGsmSmsController> _logger;

        public NetGsmSmsController(NetGsmSmsService smsService, ILogger<NetGsmSmsController> logger)
        {
            _smsService = smsService;
            _logger = logger;
        }

        [HttpGet("inbox")]
        public async Task<ActionResult<List<InboxSmsResponse>>> GetIncomingSms()
        {
            try
            {
                var smsList = await _smsService.GetIncomingSmsAsync();
                return Ok(smsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "📛 Gelen SMS'ler alınırken hata oluştu.");
                return StatusCode(500, "SMS verileri alınamadı.");
            }
        }
    }
}
