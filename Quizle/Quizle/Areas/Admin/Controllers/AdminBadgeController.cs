using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quizle.Core.Contracts;
using Quizle.Core.Exceptions;
using Quizle.Core.Models;
using Quizle.DB.Models;
using Quizle.Web.Models;

namespace Quizle.Web.Areas.Admin.Controllers
{
    public class AdminBadgeController : BaseAdminController
    {
        private readonly IBadgeService _badgeService;

        public AdminBadgeController(IBadgeService badgeService)
        {
            _badgeService = badgeService;            
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
                return RedirectToAction("Add", "AdminBadge");
            }
            if (model.Image.Length == 0 || model.Image.Length > 5242880)
            {
                return RedirectToAction("Add", "AdminBadge");
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
            try
            {
                await _badgeService.AddBadgeAsync(dto);
            }
            catch (ArgumentNullException)
            {
                BadRequest();
            }
            TempData["message"] = "You have successfully added a badge";
            return RedirectToAction("Add", "AdminBadge");
        }
        [HttpGet]
        public IActionResult Manage()
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
        [HttpPost]
        public async Task<IActionResult> Remove(int badgeId)
        {                       
            try
            {
                await _badgeService.DeleteBadgeAsync(badgeId);

            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            TempData["message"] = "Successfully deleted badge";
            return RedirectToAction("Manage", "AdminBadge", "Admin");
        }
    }
}
