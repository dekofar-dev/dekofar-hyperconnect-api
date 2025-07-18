using Dekofar.HyperConnect.Integrations.NetGsm.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Integrations.NetGsm.Services
{
    public class NetGsmSmsService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ILogger<NetGsmSmsService> _logger;

        public NetGsmSmsService(IConfiguration configuration, ILogger<NetGsmSmsService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public async Task<List<InboxSmsResponse>> GetIncomingSmsAsync()
        {
            var username = _configuration["NetGsm:Username"];
            var password = _configuration["NetGsm:Password"];
            var baseUrl = "https://api.netgsm.com.tr/sms/list/json";

            var url = $"{baseUrl}?usercode={username}&password={password}";

            _logger.LogInformation("🔗 NetGSM SMS İsteği: {Url}", url);

            HttpResponseMessage response;
            string content;

            try
            {
                response = await _httpClient.GetAsync(url);
                content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("📥 NetGSM SMS Yanıt: {Content}", content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "📛 NetGSM SMS istek hatası");
                throw new Exception("NetGSM SMS servisine bağlantı kurulamadı.");
            }

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("❌ NetGSM SMS alınamadı. StatusCode: {StatusCode}", response.StatusCode);
                throw new Exception("NetGSM SMS alınamadı.");
            }

            try
            {
                var smsList = JsonSerializer.Deserialize<List<InboxSmsResponse>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return smsList ?? new List<InboxSmsResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ NetGSM SMS JSON parse hatası");
                throw new Exception("NetGSM SMS yanıtı beklenen formatta değil.");
            }
        }
    }
}
