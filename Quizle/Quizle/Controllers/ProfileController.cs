using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Models;
using Quizle.DB.Common;
using Quizle.DB.Models;
using Quizle.Web.Models;
using System.Security.Claims;

namespace Quizle.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }
        public async Task<IActionResult> Profile()
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await _profileService.GetCurrentUser(currentUserId);
            if (user == null)
            {
                return RedirectToAction("Login", "Identity");
            }
            var userViewModel = new ProfileViewModel()
            {
                Username = user.Username,
                Email = user.Email,
                AnsweredQuestionsCount = user.AnsweredQuestionsCount,
                CurrentQuestionStatus = user.CurrentQuestionStatus ? "Has Answered" : "Not Answered",
                WinratePercent = user.WinratePercent,
                AnsweredQuestions = user.AnsweredQuestions.Select(a => new UserQuestionViewModel()
                {
                    Id = a.Id,
                    CorrectAnswer = a.CorrectAnswer,
                    SelectedAnswer = a.SelectedAnswer,
                    Question = a.Question,
                    Difficulty = a.Difficulty,
                    Type = a.Type
                }).ToList(),
                Badge = new BadgeViewModel()
                {
                    Id = user.Badge.Id,
                    Name = user.Badge.Name,
                    Description = user.Badge.Description,
                    Rarity = user.Badge.Rarity,                    
                }
            };
            if (user.Badge != null)
            {
                var photoStr = Convert.ToBase64String(user.Badge.Image);
                var imageString = string.Format("data:image/jpg;base64,{0}", photoStr);
                userViewModel.Badge.Image = imageString;
            }            
            
            return View(userViewModel);
        }
        public IActionResult Leaderboard()
        {
            return View();
        }
    }
}
