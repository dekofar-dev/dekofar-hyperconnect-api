using Dekofar.HyperConnect.Integrations.Shopify.Interfaces;
using Dekofar.HyperConnect.Integrations.Shopify.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
            _logger.LogInformation("\ud83d\udd10 Shopify bağlantı başarılı. Yanıt: {Content}", content);
            return content;
        }

        public async Task<PagedResult<Order>> GetOrdersPagedAsync(string? pageInfo = null, int limit = 10, CancellationToken ct = default)
        {
            var url = $"/admin/api/2024-04/orders.json?status=open&limit={limit}";

            if (!string.IsNullOrEmpty(pageInfo))
                url += $"&page_info={WebUtility.UrlEncode(pageInfo)}";

            var resp = await _httpClient.GetAsync(url, ct);
            resp.EnsureSuccessStatusCode();

            var content = await resp.Content.ReadAsStringAsync();
            _logger.LogInformation("\ud83d\udce6 Shopify sayfalı sipariş yanıtı: {Content}", content);

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

        public async Task<Order?> GetOrderByIdAsync(long orderId, CancellationToken ct = default)
        {
            try
            {
                var url = $"/admin/api/2024-04/orders/{orderId}.json";
                var response = await _httpClient.GetAsync(url, ct);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync(ct);
                _logger.LogInformation("\ud83d\udcc4 Shopify sipariş detayı (ham JSON): {Content}", content);

                var parsed = JsonConvert.DeserializeObject<Dictionary<string, Order>>(content);

                if (parsed != null && parsed.TryGetValue("order", out var order))
                    return order;

                _logger.LogWarning("\u26a0\ufe0f Sipariş detayı boş veya 'order' alanı eksik. ID: {OrderId}", orderId);
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "\u274c Shopify API hatası - OrderId: {OrderId}", orderId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\u274c Shopify sipariş detayı çekme hatası - ID: {OrderId}", orderId);
                throw new Exception("Shopify sipariş detayı çekilemedi.");
            }
        }
        private async Task<string?> GetImageUrlFromProductAsync(long productId, long? variantId = null, CancellationToken ct = default)
        {
            var productResp = await _httpClient.GetAsync($"/admin/api/2024-04/products/{productId}.json", ct);
            productResp.EnsureSuccessStatusCode();

            var content = await productResp.Content.ReadAsStringAsync(ct);
            dynamic productData = JsonConvert.DeserializeObject<dynamic>(content);
            var product = productData?.product;

            if (product == null || product.images == null)
                return null;

            long? imageId = null;

            // 🔍 1. variant_id varsa, karşılık gelen image_id'yi bul
            if (variantId != null && product.variants != null)
            {
                foreach (var variant in product.variants)
                {
                    if ((long)variant.id == variantId)
                    {
                        imageId = variant.image_id;
                        break;
                    }
                }
            }

            // 🎯 2. image_id varsa: product.images[] içinde eşleşeni bul
            if (imageId != null)
            {
                foreach (var img in product.images)
                {
                    if ((long)img.id == imageId)
                        return (string)img.src;
                }
            }

            // 🖼️ 3. Yoksa ürünün ilk görselini döndür
            if (product.images.Count > 0)
                return (string)product.images[0].src;

            return null;
        }




        public async Task<ShopifyOrderDetailDto?> GetOrderDetailWithImagesAsync(long orderId, CancellationToken ct = default)
        {
            var order = await GetOrderByIdAsync(orderId, ct); // REST ile siparişi çek
            if (order == null) return null;

            var lineItems = new List<ShopifyOrderDetailDto.LineItemDto>();

            foreach (var item in order.LineItems)
            {
                string? imageUrl = null;

                try
                {
                    var productId = item.ProductId;
                    if (productId <= 0) continue;

                    var productResp = await _httpClient.GetAsync($"/admin/api/2024-04/products/{productId}.json", ct);
                    productResp.EnsureSuccessStatusCode();

                    var content = await productResp.Content.ReadAsStringAsync(ct);
                    dynamic productData = JsonConvert.DeserializeObject<dynamic>(content);
                    var product = productData?.product;

                    if (product == null || product.images == null) continue;

                    long? imageId = null;

                    // ✅ VARIANT_ID üzerinden image_id tespit et
                    if (item.VariantId.HasValue && product.variants != null)
                    {
                        foreach (var variant in product.variants)
                        {
                            if ((long)variant.id == item.VariantId.Value)
                            {
                                imageId = variant.image_id;
                                break;
                            }
                        }
                    }

                    // 🔍 image_id eşleşirse görseli bul
                    if (imageId != null)
                    {
                        foreach (var img in product.images)
                        {
                            if ((long)img.id == imageId)
                            {
                                imageUrl = (string)img.src;
                                break;
                            }
                        }
                    }

                    // 🔁 eşleşme olmazsa ilk görseli al
                    if (imageUrl == null && product.images.Count > 0)
                    {
                        imageUrl = (string)product.images[0].src;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"🖼️ Ürün görseli alınamadı (ProductId: {item.ProductId})");
                }

                lineItems.Add(new ShopifyOrderDetailDto.LineItemDto
                {
                    Title = item.Title,
                    VariantTitle = item.VariantTitle,
                    Quantity = item.Quantity,
                    ImageUrl = imageUrl
                });
            }

            return new ShopifyOrderDetailDto
            {
                OrderId = order.Id,
                OrderNumber = order.Name,
                CreatedAt = DateTime.Parse(order.CreatedAt),
                Currency = order.Currency,
                TotalPrice = order.TotalPrice,
                FinancialStatus = order.FinancialStatus,
                FulfillmentStatus = order.FulfillmentStatus,
                Note = order.Note,
                NoteAttributes = order.NoteAttributes, // ✅ EKLENDİ
                Tags = order.Tags,

                Customer = new CustomerDto
                {
                    Id = order.Customer?.Id ?? 0,
                    FirstName = order.Customer?.FirstName,
                    LastName = order.Customer?.LastName,
                    Phone = order.Customer?.Phone,
                    Email = order.Customer?.Email,
                    OrdersCount = order.Customer?.OrdersCount ?? 0
                },

                BillingAddress = new AddressDto
                {
                    FirstName = order.BillingAddress?.FirstName,
                    LastName = order.BillingAddress?.LastName,
                    Address1 = order.BillingAddress?.Address1,
                    Address2 = order.BillingAddress?.Address2,
                    City = order.BillingAddress?.City,
                    Province = order.BillingAddress?.Province,
                    Country = order.BillingAddress?.Country,
                    Zip = order.BillingAddress?.Zip,
                    Phone = order.BillingAddress?.Phone
                },

                LineItems = lineItems
            };








        }
    }
}