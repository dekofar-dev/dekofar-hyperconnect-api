using Dekofar.HyperConnect.Integrations.Shopify.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Dekofar.API.Controllers.Integrations
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopifyController : ControllerBase
    {
        private readonly IShopifyService _shopifyService;

        public ShopifyController(IShopifyService shopifyService)
        {
            _shopifyService = shopifyService;
        }

        // ✅ 1. Bağlantı testi
        [HttpGet("test")]
        public async Task<IActionResult> TestConnection()
        {
            var result = await _shopifyService.TestConnectionAsync();
            return Ok(result);
        }

        // ✅ 2. Sipariş detay (görseller dahil)
        [HttpGet("order-detail/{orderId}")]
        public async Task<IActionResult> GetOrderDetail(long orderId)
        {
            var order = await _shopifyService.GetOrderDetailWithImagesAsync(orderId);
            if (order == null)
                return NotFound(new { message = "Sipariş detayı bulunamadı" });

            return Ok(order);
        }

        // ✅ 3. Sipariş (görselsiz, sade)
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetOrderById(long orderId)
        {
            var order = await _shopifyService.GetOrderByIdAsync(orderId);
            if (order == null)
                return NotFound(new { message = "Sipariş bulunamadı" });

            return Ok(order);
        }

        // ✅ 4. Tüm ürünler
        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts(CancellationToken ct = default)
        {
            var products = await _shopifyService.GetAllProductsAsync(ct);
            return Ok(products);
        }

        // ✅ 5. Sayfalı sipariş listesi
        [HttpGet("orders-paged")]
        public async Task<IActionResult> GetPagedOrders([FromQuery] int limit = 10, [FromQuery] string? pageInfo = null)
        {
            try
            {
                var result = await _shopifyService.GetOrdersPagedAsync(pageInfo, limit);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(new
                {
                    error = "Shopify API hatası",
                    message = ex.Message
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Sunucu hatası",
                    message = ex.Message
                });
            }
        }

        // ✅ 6. Ürün arama
        [HttpGet("search-products")]
        public async Task<IActionResult> SearchProducts([FromQuery] string query, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(new { message = "Arama sorgusu boş olamaz." });

            var result = await _shopifyService.SearchProductsAsync(query, ct);
            return Ok(result);
        }

        // ✅ 7. Varyant detay
        [HttpGet("variant/{variantId}")]
        public async Task<IActionResult> GetVariantById(long variantId, CancellationToken ct = default)
        {
            var variant = await _shopifyService.GetVariantByIdAsync(variantId, ct);
            if (variant == null)
                return NotFound(new { message = "Varyant bulunamadı." });

            return Ok(variant);
        }

        // ✅ 8. Ürünün tüm varyantları
        [HttpGet("variants-by-product/{productId}")]
        public async Task<IActionResult> GetVariantsByProductId(long productId, CancellationToken ct = default)
        {
            var variants = await _shopifyService.GetVariantsByProductIdAsync(productId, ct);
            return Ok(variants);
        }

        // ✅ 9. Kritik stok ürünleri
        [HttpGet("low-stock-products")]
        public async Task<IActionResult> GetLowStockProducts([FromQuery] int threshold = 5, CancellationToken ct = default)
        {
            var products = await _shopifyService.GetLowStockProductsAsync(threshold, ct);
            return Ok(products);
        }

        // ✅ 10. Ürün etiket güncelle
        [HttpPut("update-product-tags/{productId}")]
        public async Task<IActionResult> UpdateProductTags(long productId, [FromBody] string tags, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(tags))
                return BadRequest(new { message = "Etiket değeri boş olamaz." });

            var success = await _shopifyService.AddOrUpdateProductTagsAsync(productId, tags, ct);
            if (!success)
                return StatusCode(500, new { message = "Etiket güncelleme işlemi başarısız oldu." });

            return Ok(new { message = "Etiketler başarıyla güncellendi." });
        }
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetProductById(long productId, CancellationToken ct = default)
        {
            var product = await _shopifyService.GetProductByIdAsync(productId, ct);
            if (product == null)
                return NotFound(new { message = "Ürün bulunamadı." });

            return Ok(product);
        }

    }
}
