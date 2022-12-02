using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quizle.Core.Contracts;
using Quizle.Core.Models;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public BadgeController(IBadgeService badgeService, UserManager<ApplicationUser> userManager)
        {
            _badgeService = badgeService;
            _userManager = userManager;
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
        
        [HttpGet]
        public IActionResult Add()
        {
            var model = new BadgeAddViewModel()
            {
                Rarities = _badgeService.GetRarities()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(BadgeAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Add", "Badge");
            }
            if (model.Image.Length == 0)
            {
                return BadRequest();
            }
            if (model.Image.Length > 5242880)
            {
                return BadRequest();
            }
            var dto = new BadgeDto()
            {
                Name = model.Name,
                Rarity = model.Rarity,
                Description = model.Description,
                Price = model.Price
            };
            using (var memoryStream = new MemoryStream())
            {
                await model.Image.CopyToAsync(memoryStream);
                dto.Image = memoryStream.ToArray();
            }
            await _badgeService.AddBadgeAsync(dto);
            return RedirectToAction("Add", "Badge");
        }
        public async Task<IActionResult> Buy(int badgeId, int badgePrice)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var user = await _userManager.FindByIdAsync(userId);
                if (user.CurrentQuizPoints >= badgePrice)
                {
                    await _badgeService.BuyBadgeAsync(badgeId, userId);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return RedirectToAction(nameof(All));
        }
    }
}
