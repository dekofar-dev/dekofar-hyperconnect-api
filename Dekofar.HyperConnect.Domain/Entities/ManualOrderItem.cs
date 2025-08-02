using System;

namespace Dekofar.HyperConnect.Domain.Entities
{
    public class ManualOrderItem
    {
        public Guid Id { get; set; }
        public Guid ManualOrderId { get; set; }
        public string ProductId { get; set; } = default!;
        public string ProductName { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }

        public ManualOrder? ManualOrder { get; set; }
    }
}

