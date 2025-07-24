namespace Dekofar.HyperConnect.Integrations.Shopify.Models
{
    public class CustomerDto
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int OrdersCount { get; set; }
    }
}
