using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Dekofar.HyperConnect.Domain.Entities;

namespace Dekofar.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; }
        //public string? Role { get; set; }

        /// <summary>
        ///     URL or file path of the user's profile image.
        /// </summary>
        public string? AvatarUrl { get; set; }

        public DateTime MembershipDate { get; set; }
        public bool IsOnline { get; set; }
        public DateTime? LastSeen { get; set; }
        public int TotalSalesCount { get; set; }
        public decimal TotalCommissionAmount { get; set; }
        public int TotalSupportRequestCount { get; set; }
        public int UnreadMessageCount { get; set; }
        public DateTime? LastMessageDate { get; set; }
        public DateTime? LastSupportActivity { get; set; }

        [NotMapped]
        public int TotalSupportTickets
        {
            get => TotalSupportRequestCount;
            set => TotalSupportRequestCount = value;
        }

        [NotMapped]
        public int OpenSupportTickets { get; set; }

        [NotMapped]
        public int ClosedSupportTickets { get; set; }

        [NotMapped]
        public DateTime? LastSupportActivityAt
        {
            get => LastSupportActivity;
            set => LastSupportActivity = value;
        }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Commission> Commissions { get; set; } = new List<Commission>();
    }
}
