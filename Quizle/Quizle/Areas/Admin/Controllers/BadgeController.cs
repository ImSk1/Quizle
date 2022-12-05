using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quizle.Core.Contracts;
using Quizle.Core.Models;
using Quizle.DB.Models;
using Quizle.Web.Models;

namespace Quizle.Web.Areas.Admin.Controllers
{
    public class BadgeController : BaseAdminController
    {
        private readonly IBadgeService _badgeService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BadgeController(IBadgeService badgeService, UserManager<ApplicationUser> userManager)
        {
            _badgeService = badgeService;
            _userManager = userManager;
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
    }
}
