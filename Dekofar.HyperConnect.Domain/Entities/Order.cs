using System;
using Dekofar.Domain.Entities;

namespace Dekofar.HyperConnect.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }

        public Guid? SellerId { get; set; }
        public ApplicationUser? Seller { get; set; }

        // Optional: integration with other systems could go here
    }
}
