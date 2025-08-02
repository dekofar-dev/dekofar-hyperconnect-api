using Microsoft.AspNetCore.Identity;
using System;

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

    }
}
