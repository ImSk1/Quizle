using Microsoft.AspNetCore.Mvc;
using Quizle.Core.Contracts;
using Quizle.Core.Models;
using Quizle.Web.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Quizle.Web.Controllers
{
    public class BadgeController : Controller
    {
        private readonly IBadgeService _badgeService;

        public BadgeController(IBadgeService badgeService)
        {
            _badgeService = badgeService;
        }

        public IActionResult All()
        {
            var badgeDtos = _badgeService.GetAllBadges();
            var models = new List<BadgeViewModel>();
            foreach (var badgeDto in badgeDtos)
            {
                var model = new BadgeViewModel()
                {
                    Name = badgeDto.Name,
                    Description = badgeDto.Description,
                    Rarity = badgeDto.Rarity,
                };
                var photoStr = Convert.ToBase64String(badgeDto.Image);
                var imageString = string.Format("data:image/jpg;base64,{0}", photoStr);
                model.Image = imageString;
                models.Add(model);
            }
            return View(models);

        }
        public IActionResult AddToCollection(int badgeId)
        {
            return View();
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
                Description = model.Description
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
