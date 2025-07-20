
namespace Dekofar.HyperConnect.Integrations.Shopify.Interfaces
{
    public interface IShopifyService
    {
        Task<string> TestConnectionAsync(CancellationToken cancellationToken = default);
        Task<PagedResult<Order>> GetOrdersPagedAsync(string? pageInfo = null, int limit = 10, CancellationToken ct = default);
        Task<Order?> GetOrderByIdAsync(long orderId, CancellationToken ct = default);
    }

}
