using Dekofar.HyperConnect.Integrations.Shopify.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
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

        [HttpGet("test")]
        public async Task<IActionResult> TestConnection()
        {
            var result = await _shopifyService.TestConnectionAsync();
            return Ok(result);
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetOrderById(long orderId)
        {
            var order = await _shopifyService.GetOrderByIdAsync(orderId);
            if (order == null)
                return NotFound(new { message = "Sipariş bulunamadı" });

            return Ok(order);
        }


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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Sunucu hatası",
                    message = ex.Message
                });
            }
        }


    }
}
