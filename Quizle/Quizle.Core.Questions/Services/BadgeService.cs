using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Exceptions;
using Quizle.Core.Models;
using Quizle.DB.Common;
using Quizle.DB.Common.Enums;
using Quizle.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Services
{
    public class BadgeService : IBadgeService
    {
        private readonly IRepository _repo;

        public BadgeService(IRepository repo)
        {
            _repo = repo;
        }

        public List<BadgeDto> GetAllBadges()
        {
            var badges = _repo
                .All<Badge>()
                .Include(a => a.ApplicationUsersBadges)
                .ThenInclude(a => a.ApplicationUser)
                .Select(a => new BadgeDto()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Rarity = a.Rarity.ToString(),
                    Image = a.Image,
                    OwnerIds = a.ApplicationUsersBadges.Select(a => a.ApplicationUserId).ToArray(),
                    Price = a.Price
                }).ToList();            
            return badges;
        }
        public List<string> GetRarities()
        {
            var rarities = new List<string>();
            foreach (var rarity in (Rarity[])Enum.GetValues(typeof(Rarity)))
            {
                rarities.Add(rarity.ToString());
            }
            return rarities;
        }

        public async Task AddBadgeAsync(BadgeDto badge)
        {
            var entity = new Badge()
            {
                Name = badge.Name,
                Description = badge.Description,
                Rarity = (Rarity)Enum.Parse(typeof(Rarity), badge.Rarity),
                Image = badge.Image,
                Price = badge.Price
            };
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
        }
        public async Task BuyBadgeAsync(int badgeId, string userId)
        {
            var user = await _repo.All<ApplicationUser>(a => a.Id == userId)                
                .Include(u => u.ApplicationUsersBadges)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException();
            }
            var badge = await _repo.GetByIdAsync<Badge>(badgeId);
            if (badge == null)
            {
                throw new NotFoundException();
            }
            if (!user.ApplicationUsersBadges.Any(b => b.BadgeId == badgeId))
            {
                user.ApplicationUsersBadges.Add(new ApplicationUserBadge()
                {
                    Badge = badge,
                    ApplicationUserId = user.Id,
                    BadgeId = badge.Id,
                    ApplicationUser = user
                });
                await _repo.SaveChangesAsync();
            }
        }

    }
}
