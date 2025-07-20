using Dekofar.HyperConnect.Integrations.NetGsm.Interfaces;
using Dekofar.HyperConnect.Integrations.NetGsm.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Dekofar.HyperConnect.Integrations.NetGsm.Services
{
    public class NetGsmSmsService : INetGsmSmsService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<NetGsmSmsService> _logger;
        private readonly HttpClient _httpClient;

        public NetGsmSmsService(IConfiguration configuration, ILogger<NetGsmSmsService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public async Task<List<SmsInboxResponse>> GetInboxMessagesAsync(SmsInboxRequest request)
        {
            var username = _configuration["NetGsm:Username"];
            var password = _configuration["NetGsm:Password"];
            var baseUrl = "https://api.netgsm.com.tr/sms/rest/v2/inbox";

            // Basic Auth header
            var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            // x-www-form-urlencoded content
            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("startdate", request.StartDate),
                new KeyValuePair<string, string>("stopdate", request.StopDate)
            });

            try
            {
                _logger.LogInformation("📥 NetGSM Gelen SMS isteği: {Url}", baseUrl);

                var response = await _httpClient.PostAsync(baseUrl, formData);
                var content = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("📥 NetGSM Yanıt: {Content}", content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"NetGSM SMS inbox hatası: {response.StatusCode}");
                }

                var json = JsonSerializer.Deserialize<Dictionary<string, List<SmsInboxResponse>>>(content);
                return json != null && json.TryGetValue("inbox", out var inboxList) ? inboxList : new List<SmsInboxResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ NetGSM SMS inbox çekme hatası");
                throw;
            }
        }
    }
}
