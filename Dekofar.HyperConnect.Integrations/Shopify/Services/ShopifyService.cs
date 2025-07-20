using Dekofar.HyperConnect.Integrations.Shopify.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Integrations.Shopify.Services
{
    public class ShopifyService : IShopifyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ShopifyService> _logger;
        private readonly string _baseUrl;
        private readonly string _accessToken;

        public ShopifyService(HttpClient httpClient, IConfiguration configuration, ILogger<ShopifyService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _baseUrl = configuration["Shopify:BaseUrl"];
            _accessToken = configuration["Shopify:AccessToken"];

            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-Shopify-Access-Token", _accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> TestConnectionAsync(CancellationToken cancellationToken = default)
        {
            var resp = await _httpClient.GetAsync("/admin/api/2024-04/shop.json", cancellationToken);
            resp.EnsureSuccessStatusCode();
            var content = await resp.Content.ReadAsStringAsync();
            _logger.LogInformation("🔐 Shopify bağlantı başarılı. Yanıt: {Content}", content);
            return content;
        }

        public async Task<PagedResult<Order>> GetOrdersPagedAsync(string? pageInfo = null, int limit = 10, CancellationToken ct = default)
        {
            var url = "/admin/api/2024-01/orders.json?status=open&limit=250";

            if (!string.IsNullOrEmpty(pageInfo))
                url += $"&page_info={WebUtility.UrlEncode(pageInfo)}";

            var resp = await _httpClient.GetAsync(url, ct);
            resp.EnsureSuccessStatusCode();

            var content = await resp.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<OrdersResponse>(content);

            string? nextPageInfo = null;

            if (resp.Headers.TryGetValues("Link", out var linkHeaders))
            {
                var linkHeader = linkHeaders.FirstOrDefault();
                var match = Regex.Match(linkHeader ?? "", @"page_info=([^&>]+)");
                if (match.Success)
                    nextPageInfo = match.Groups[1].Value;
            }

            return new PagedResult<Order>
            {
                Items = result?.Orders ?? new(),
                NextPageInfo = nextPageInfo
            };
        }

        // Services/ShopifyService.cs
        public async Task<Order?> GetOrderByIdAsync(long orderId, CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/admin/api/2024-04/orders/{orderId}.json", ct);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync(ct);

                var json = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, Order>>(content);
                return json != null && json.TryGetValue("order", out var order) ? order : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Shopify sipariş detayı çekme hatası - ID: {OrderId}", orderId);
                throw new Exception("Shopify sipariş detayı çekilemedi.");
            }
        }
    }
}
