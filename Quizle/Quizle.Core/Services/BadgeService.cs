using Microsoft.AspNetCore.Identity;
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
		private readonly UserManager<ApplicationUser> _userManager;

		public BadgeService(IRepository repo, UserManager<ApplicationUser> userManager)
		{
			_repo = repo;
			_userManager = userManager;
		}
		public bool ExistsById(int id) => _repo.GetByIdAsync<Badge>(id) != null;
		public bool UserOwnsBadge(string userId, int badgeId) => _repo.AllReadonly<ApplicationUserBadge>().Any(a => a.BadgeId == badgeId && a.ApplicationUserId == userId);
		public async Task<bool> UserOwnsBetterBadge(string userId, int badgeId)
		{
			if (_userManager.FindByIdAsync(userId) == null)
			{
				throw new NotFoundException("User doesn't exist.");
			}
			var userBadges = _repo.AllReadonly<ApplicationUserBadge>().Where(a => a.ApplicationUserId == userId).Include(a => a.Badge).ToList();
			if (!ExistsById(badgeId))
			{
				throw new NotFoundException("Badge doesn't exist.");
			}
			var badge = await _repo.GetByIdAsync<Badge>(badgeId);
			if (userBadges.Count == 0)
			{
				return false;
			}
			if (userBadges.Any(a => ((int?)a.Badge?.Rarity ?? 0) > (int)badge.Rarity))
			{
				return true;
			}
			else
			{
				return false;
			}
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
					OwnerIds = a.ApplicationUsersBadges.Select(a => a.ApplicationUserId).ToList(),
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
			//ADD VALIDATION
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
        public async Task DeleteBadgeAsync(int badgeId)
        {			
            if (!ExistsById(badgeId))
            {
                throw new NotFoundException();
            }            
            await _repo.DeleteAsync<Badge>(badgeId);
            await _repo.SaveChangesAsync();
        }
        public async Task BuyBadgeAsync(int badgeId, string userId)
		{
			if (string.IsNullOrEmpty(userId))
			{
				throw new ArgumentException();
			}

			var user = _userManager.Users
			.Include(u => u.ApplicationUsersBadges)
			.FirstOrDefault(a => a.Id == userId);

			if (user == null)
			{
				throw new NotFoundException("User not found.");
			}

			var badge = await _repo.GetByIdAsync<Badge>(badgeId);

			if (badge == null)
			{
				throw new NotFoundException("Badge not found.");
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
