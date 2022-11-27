using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Models;
using Quizle.DB.Common;
using Quizle.DB.Common.Enums;
using Quizle.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var list = _repo
                .All<Badge>()
                .Include(a => a.ApplicationUsersBadges)
                .ThenInclude(a => a.ApplicationUser)
                .Select(a => new BadgeDto()
                {
                    Name = a.Name,
                    Description = a.Description,
                    Rarity = a.Rarity.ToString(),
                    Image = a.Image,
                    OwnerIds = a.ApplicationUsersBadges.Select(a => a.ApplicationUserId).ToArray()
                }).ToList();
            return list;
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
                Image = badge.Image
            };
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
        }
    }
}
