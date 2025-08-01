using Dekofar.HyperConnect.Integrations.Shopify.Interfaces;
using Dekofar.HyperConnect.Integrations.Shopify.Models;
using Dekofar.HyperConnect.Integrations.Shopify.Models.Shopify;
using Dekofar.HyperConnect.Integrations.Shopify.Models.Shopify.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Dekofar.HyperConnect.API.Controllers.Shopify
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

        [HttpGet("test-connection")]
        public async Task<IActionResult> TestConnection(CancellationToken ct)
        {
            var result = await _shopifyService.TestConnectionAsync(ct);
            return Ok(result);
        }

        [HttpGet("orders-paged")]
        public async Task<IActionResult> GetOrdersPaged([FromQuery] string? pageInfo, [FromQuery] int limit = 10, CancellationToken ct = default)
        {
            var result = await _shopifyService.GetOrdersPagedAsync(pageInfo, limit, ct);
            return Ok(result);
        }

        [HttpGet("orders/{orderId:long}")]
        public async Task<IActionResult> GetOrderById(long orderId, CancellationToken ct)
        {
            var order = await _shopifyService.GetOrderByIdAsync(orderId, ct);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpGet("order-detail/{orderId:long}")]
        public async Task<IActionResult> GetOrderDetailWithImages(long orderId, CancellationToken ct)
        {
            var detail = await _shopifyService.GetOrderDetailWithImagesAsync(orderId, ct);
            if (detail == null)
                return NotFound();
            return Ok(detail);
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts(CancellationToken ct)
        {
            var products = await _shopifyService.GetAllProductsAsync(ct);
            return Ok(products);
        }

        [HttpGet("products/{productId:long}")]
        public async Task<IActionResult> GetProductById(long productId, CancellationToken ct)
        {
            var product = await _shopifyService.GetProductByIdAsync(productId, ct);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpGet("products/search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string query, CancellationToken ct)
        {
            var result = await _shopifyService.SearchProductsAsync(query, ct);
            return Ok(result);
        }

        [HttpGet("variants/{variantId:long}")]
        public async Task<IActionResult> GetVariantById(long variantId, CancellationToken ct)
        {
            var variant = await _shopifyService.GetVariantByIdAsync(variantId, ct);
            if (variant == null)
                return NotFound();
            return Ok(variant);
        }

        [HttpGet("products/{productId:long}/variants")]
        public async Task<IActionResult> GetVariantsByProductId(long productId, CancellationToken ct)
        {
            var variants = await _shopifyService.GetVariantsByProductIdAsync(productId, ct);
            return Ok(variants);
        }

        [HttpGet("products/low-stock")]
        public async Task<IActionResult> GetLowStockProducts([FromQuery] int threshold = 5, CancellationToken ct = default)
        {
            var products = await _shopifyService.GetLowStockProductsAsync(threshold, ct);
            return Ok(products);
        }

        [HttpPut("products/{productId:long}/tags")]
        public async Task<IActionResult> AddOrUpdateProductTags(long productId, [FromQuery] string tags, CancellationToken ct)
        {
            var success = await _shopifyService.AddOrUpdateProductTagsAsync(productId, tags, ct);
            if (success)
                return Ok(new { message = "Etiketler başarıyla güncellendi." });
            return BadRequest(new { message = "Etiket güncelleme başarısız oldu." });
        }

        [HttpPost("order/update-tags")]
        public async Task<IActionResult> UpdateOrderTags([FromBody] UpdateOrderTagsRequest request, CancellationToken ct)
        {
            var ok = await _shopifyService.UpdateOrderTagsAsync(request.OrderId, request.Tags, ct);
            return ok ? Ok() : BadRequest();
        }

        [HttpPost("order/update-note")]
        public async Task<IActionResult> UpdateOrderNote([FromBody] UpdateOrderNoteRequest request, CancellationToken ct)
        {
            var ok = await _shopifyService.UpdateOrderNoteAsync(request.OrderId, request.Note, ct);
            return ok ? Ok() : BadRequest();
        }

        [HttpGet("customer/{customerId:long}")]
        public async Task<IActionResult> GetCustomer(long customerId, CancellationToken ct)
        {
            var customer = await _shopifyService.GetCustomerByIdAsync(customerId, ct);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpPost("order")]
        public async Task<IActionResult> CreateOrder([FromBody] Order order, CancellationToken ct)
        {
            var result = await _shopifyService.CreateOrderAsync(order, ct);
            return result != null ? Ok(result) : BadRequest();
        }

        [HttpPost("fulfillment")]
        public async Task<IActionResult> CreateFulfillment([FromBody] FulfillmentCreateRequest request, CancellationToken ct)
        {
            var resp = await _shopifyService.CreateFulfillmentAsync(request.OrderId, request, ct);
            return Ok(resp);
        }

        [HttpGet("orders/search")]
        public async Task<IActionResult> SearchOrders([FromQuery] string query, CancellationToken ct)
        {
            var result = await _shopifyService.SearchOrdersAsync(query, ct);
            return Ok(result);
        }

        [HttpGet("orders-open-cursor")]
        public async Task<IActionResult> GetOpenOrdersWithCursor([FromQuery] string? pageInfo, [FromQuery] int limit = 20, CancellationToken ct = default)
        {
            var result = await _shopifyService.GetOpenOrdersWithCursorAsync(pageInfo, limit, ct);
            return Ok(result);
        }
        [HttpPost("orders/clear-cache")]
        public IActionResult ClearOrderCache([FromServices] IMemoryCache memoryCache)
        {
            memoryCache.Remove("shopify_orders_cache");
            return Ok(new { message = "🧹 Cache temizlendi." });
        }


    }
}
