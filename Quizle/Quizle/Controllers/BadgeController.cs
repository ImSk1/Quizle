using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quizle.Core.Contracts;
using Quizle.Core.Exceptions;
using Quizle.Core.Models;
using Quizle.Core.Services;
using Quizle.DB.Models;
using Quizle.Web.Models;
using System.Net;
using System.Security.Claims;

namespace Quizle.Web.Controllers
{
    [Authorize]
    public class BadgeController : Controller
    {
        private readonly IBadgeService _badgeService;
        private readonly IProfileService _profileService;

        public BadgeController(IBadgeService badgeService, IProfileService profileService)
        {
            _badgeService = badgeService;
            _profileService = profileService;
        }

        public IActionResult All()
        {
            var badgeDtos = _badgeService.GetAllBadges();
            var models = new List<BadgeViewModel>();
            foreach (var badgeDto in badgeDtos)
            {
                var model = new BadgeViewModel()
                {
                    Id = badgeDto.Id,
                    Name = badgeDto.Name,
                    Description = badgeDto.Description,
                    Rarity = badgeDto.Rarity,
                    Price = badgeDto.Price,
                    OwnerIds = badgeDto.OwnerIds
                };
                var photoStr = Convert.ToBase64String(badgeDto.Image);
                var imageString = string.Format("data:image/jpg;base64,{0}", photoStr);
                model.Image = imageString;
                models.Add(model);
            }
            return View(models);

        }
        public IActionResult Mine()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return BadRequest();
            }
            var userBadgeDtos = _badgeService.GetAllMine(userId);
            var models = new List<UserBadgeViewModel>();
            foreach (var userBadgeDto in userBadgeDtos)
            {
                var userBadge = new UserBadgeViewModel()
                {
                    Badge = new BadgeViewModel()
                    {
                        Id = userBadgeDto.Badge.Id,
                        Name = userBadgeDto.Badge.Name,
                        Description = userBadgeDto.Badge.Description,
                        Rarity = userBadgeDto.Badge.Rarity,
                        Price = userBadgeDto.Badge.Price,
                        OwnerIds = userBadgeDto.Badge.OwnerIds
                    },
                    DateAcquired = userBadgeDto.DateAcquired
                   
                };
                var photoStr = Convert.ToBase64String(userBadgeDto.Badge.Image);
                var imageString = string.Format("data:image/jpg;base64,{0}", photoStr);
                userBadge.Badge.Image = imageString;
                models.Add(userBadge);
            }
            return View(models);

        }

        public async Task<IActionResult> Buy(int badgeId, int badgePrice)
        {

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var user =  _profileService.GetUser(a => a.Id == userId);
            if (_badgeService.UserOwnsBadge(user.Id, badgeId))
            {
                return RedirectToAction(nameof(All));
            }            
            if (userId == null)
            {
                return NotFound();
            }
            if (user.QuizPoints >= badgePrice)
            {

                try
                {
					await _badgeService.BuyBadgeAsync(badgeId, userId);
				}
				catch (ArgumentException)
                {

                    return BadRequest();
                }
                catch(NotFoundException) 
                {
                    return NotFound();
                }
            }
            return RedirectToAction(nameof(All));
        }
        public async Task<IActionResult> SetOnProfile(int badgeId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            await _badgeService.SetOnProfileAsync(badgeId, userId);
            return RedirectToAction(nameof(Mine));
        }
    }
}
