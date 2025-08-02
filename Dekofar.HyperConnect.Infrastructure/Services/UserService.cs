using System;
using System.Linq;
using System.Threading.Tasks;
using Dekofar.HyperConnect.Application.Interfaces;
using Dekofar.HyperConnect.Application.Users.DTOs;
using Dekofar.HyperConnect.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dekofar.HyperConnect.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbContext _dbContext;

        public UserService(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserProfileDto?> GetProfileWithStatsAsync(Guid userId)
        {
            var user = await _dbContext.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserProfileDto
                {
                    Id = u.Id,
                    FullName = u.FullName ?? string.Empty,
                    Email = u.Email!,
                    AvatarUrl = u.AvatarUrl,
                    UnreadMessageCount = _dbContext.UserMessages.Count(m => m.ReceiverId == u.Id && !m.IsRead),
                    LastMessageAt = _dbContext.UserMessages
                        .Where(m => m.SenderId == u.Id || m.ReceiverId == u.Id)
                        .OrderByDescending(m => m.SentAt)
                        .Select(m => m.SentAt)
                        .FirstOrDefault(),
                    LastSupportInteractionAt = _dbContext.SupportTickets
                        .Where(t => t.CreatedByUserId == u.Id || t.AssignedUserId == u.Id)
                        .OrderByDescending(t => t.LastUpdatedAt)
                        .Select(t => t.LastUpdatedAt)
                        .FirstOrDefault(),
                    IsOnline = u.IsOnline
                })
                .FirstOrDefaultAsync();

            return user;
        }
    }
}
