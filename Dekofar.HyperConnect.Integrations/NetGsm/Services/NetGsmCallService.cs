using Dekofar.HyperConnect.Integrations.NetGsm.Interfaces;
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
    public class NetGsmCallService : INetGsmCallService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ILogger<NetGsmCallService> _logger;

        public NetGsmCallService(IConfiguration configuration, ILogger<NetGsmCallService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public async Task<List<CallLogResponse>> GetCallLogsAsync(CallLogRequest request)
        {
            var username = _configuration["NetGsm:Username"];
            var password = _configuration["NetGsm:Password"];
            var baseUrl = "https://api.netgsm.com.tr/cdr/list/json"; // ✅ Doğru endpoint

            var url = $"{baseUrl}?usercode={username}&password={password}&startdate={request.StartDate}&stopdate={request.EndDate}";

            _logger.LogInformation("🔗 NetGSM İsteği: {Url}", url);

            HttpResponseMessage response;
            string content;

            try
            {
                response = await _httpClient.GetAsync(url);
                content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("📥 NetGSM Yanıt: {Content}", content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "📛 NetGSM'e istek atılamadı.");
                throw new Exception("NetGSM'e bağlantı kurulamadı.");
            }

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("❌ NetGSM çağrı kayıtları alınamadı. StatusCode: {StatusCode}", response.StatusCode);
                throw new Exception("NetGSM çağrı kayıtları alınamadı.");
            }

            try
            {
                var logs = JsonSerializer.Deserialize<List<CallLogResponse>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return logs ?? new List<CallLogResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ JSON deserialize hatası");
                throw new Exception("NetGSM yanıtı beklenen formatta değil.");
            }
        }
    }
}
