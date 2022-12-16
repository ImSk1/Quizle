using AutoMapper;
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
        private readonly IMapper _mapper;

        public BadgeController(IBadgeService badgeService, IProfileService profileService, IMapper mapper)
        {
            _badgeService = badgeService;
            _profileService = profileService;
            _mapper = mapper;
        }

        public IActionResult All()
        {
            var badgeDtos = _badgeService.GetAllBadges();
            var models = new List<BadgeViewModel>();
            foreach (var badgeDto in badgeDtos)
            {
                var badge = _mapper.Map<BadgeViewModel>(badgeDto);
                models.Add(badge);
            }
            return View(models);

        }
        public IActionResult Mine()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;            
            var userBadgeDtos = _badgeService.GetAllMine(userId);
            var models = new List<UserBadgeViewModel>();
            foreach (var userBadgeDto in userBadgeDtos)
            {
                var userBadge = _mapper.Map<UserBadgeViewModel>(userBadgeDto);
                models.Add(userBadge);
            }
            return View(models);

        }

        public async Task<IActionResult> Buy(int badgeId, int badgePrice)
        {

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
                                 
            await _badgeService.BuyBadgeAsync(badgeId, userId);            
            return RedirectToAction(nameof(All));
        }
        public async Task<IActionResult> SetOnProfile(int badgeId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;

            await _badgeService.SetOnProfileAsync(badgeId, userId);
            return RedirectToAction(nameof(Mine));
        }
    }
}
