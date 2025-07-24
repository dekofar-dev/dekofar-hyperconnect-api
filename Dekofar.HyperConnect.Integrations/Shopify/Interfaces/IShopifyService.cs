using Dekofar.HyperConnect.Integrations.Shopify.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Integrations.Shopify.Interfaces
{
    public interface IShopifyService
    {
        /// <summary>
        /// Shopify bağlantısını test eder
        /// </summary>
        Task<string> TestConnectionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Sayfalı sipariş listesini getirir
        /// </summary>
        Task<PagedResult<Order>> GetOrdersPagedAsync(string? pageInfo = null, int limit = 10, CancellationToken ct = default);

        /// <summary>
        /// Sipariş ID'sine göre sipariş detayını getirir
        /// </summary>
        Task<Order?> GetOrderByIdAsync(long orderId, CancellationToken ct = default);

        /// <summary>
        /// Sipariş ID'sine göre sadeleştirilmiş sipariş detayını (ürün görselleri dahil) getirir
        /// </summary>
        Task<ShopifyOrderDetailDto?> GetOrderDetailWithImagesAsync(long orderId, CancellationToken ct = default);
    }
}
