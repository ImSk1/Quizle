using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Exceptions;
using Quizle.Core.Models;
using Quizle.DB.Common;
using Quizle.DB.Models;
using Quizle.Web.Models;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
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
        
   

        public IActionResult Profile(string? username)
        {
            string name = "";
            if (username == null)
            {
                string? loggedInName = User?.Identity?.Name;
                if (loggedInName == null)
                {
                    return NotFound();

                }
                name = loggedInName;

            }
            else
            {
                name = username;
            }
            ProfileViewModel user;
            try
            {
				user = GetUser(name);
				return View(user);
			}
			catch (NotFoundException)
            {
                return NotFound();
            }             
        }
        public IActionResult Leaderboard()
        {
            var model = new List<LeaderboardProfileViewModel>();
            var dtos =  _profileService.GetTopFive();
            foreach (var dto in dtos)
            {
                model.Add(new LeaderboardProfileViewModel()
                {
                    ProfileId = dto.Id,
                    Username = dto.Username,
                    QuizPoints = dto.QuizPoints
                });
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Leaderboard(string profileId)
        {
           
            return RedirectToAction("Profile", "Profile", new {profileId = profileId});
        }
        [NonAction]
        public ProfileViewModel GetUser(string username)
        {
            ProfileDto user;
            
		    user = _profileService.GetUser(a => a.UserName == username);
          
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
                UserBadge = user.UserBadge != null ? new UserBadgeViewModel()
                {
                    Badge = new BadgeViewModel()
                    {
                        Id = user.UserBadge.Badge.Id,
                        Name = user.UserBadge.Badge.Name,
                        Description = user.UserBadge.Badge.Description,
                        Rarity = user.UserBadge.Badge.Rarity,
                    },
                    DateAcquired = user.UserBadge.DateAcquired                                        
                } : null
            };
            if (user.UserBadge != null && userViewModel.UserBadge != null)
            {
                var photoStr = Convert.ToBase64String(user.UserBadge.Badge.Image);
                var imageString = string.Format("data:image/jpg;base64,{0}", photoStr);
                userViewModel.UserBadge.Badge.Image = imageString;
            }
            return userViewModel;
        }
    }
}
