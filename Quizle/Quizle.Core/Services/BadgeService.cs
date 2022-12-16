using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quizle.Core.Common;
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
		public async Task<bool> ExistsByIdAsync(int id) => await _repo.GetByIdAsync<Badge>(id) != null;
		public bool UserOwnsBadge(string userId, int badgeId) => _repo.AllReadonly<ApplicationUserBadge>().Any(a => a.BadgeId == badgeId && a.ApplicationUserId == userId);

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
		public List<UserBadgeDto> GetAllMine(string userId)
		{
			var userBadges = _repo
				.All<ApplicationUserBadge>()
				.Include(a => a.Badge)
				.Where(a => a.ApplicationUserId == userId)
				.Select(a => new UserBadgeDto()
				{
					Badge = new BadgeDto
					{
						Id = a.Badge.Id,
						Name = a.Badge.Name,
						Description = a.Badge.Description,
						Rarity = a.Badge.Rarity.ToString(),
						Image = a.Badge.Image,
						Price = a.Badge.Price
					},
					DateAcquired = a.AcquisitionDate.ToString("dd/MM/yyyy"),

				})
				.ToList();
			return userBadges;
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
			if (!badge.IsValid())
			{
				throw new ArgumentException();
			}
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
			if (!await ExistsByIdAsync(badgeId))
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
			if (user.CurrentQuizPoints < badge.Price)
			{
                throw new CannotBuyBadgeException();
            }
			if (user.ApplicationUsersBadges.Any(b => b.BadgeId == badgeId))
			{
				throw new CannotBuyBadgeException();
			}
            user.ApplicationUsersBadges.Add(new ApplicationUserBadge()
            {
                Badge = badge,
                ApplicationUserId = user.Id,
                BadgeId = badge.Id,
                ApplicationUser = user,
                AcquisitionDate = DateTime.UtcNow,
                IsOnProfile = false
            });
            await _repo.SaveChangesAsync();
        }
		public async Task SetOnProfileAsync(int badgeId, string userId)
		{
			if (string.IsNullOrEmpty(userId))
			{
				throw new ArgumentException();
			}
			if (!await ExistsByIdAsync(badgeId))
			{
				throw new NotFoundException();
			}
			var allUserBadges = _repo.All<ApplicationUserBadge>();
			try
			{
                await allUserBadges.ForEachAsync(a => a.IsOnProfile = false);

            }
            catch (Exception)
			{

				throw;
			}
			var userBadge = await _repo.GetByCompositeKey<ApplicationUserBadge>(userId, badgeId);
			userBadge.IsOnProfile = true;
			await _repo.SaveChangesAsync();
		}
	}
}
