using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Exceptions;
using Quizle.Core.Models;
using Quizle.DB.Common;
using Quizle.DB.Models;
using Quizle.Web.Models.Profile;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Security.Claims;

namespace Quizle.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly IMapper _mapper;

        public ProfileController(IProfileService profileService, IMapper mapper)
        {
            _profileService = profileService;
            _mapper = mapper;
        }



        public IActionResult Profile(string? username)
        {
            string name = "";
            if (username == null)
            {
                string? loggedInName = User?.Identity?.Name!;
                name = loggedInName;
            }
            else
            {
                name = username;
            }
           
            var user = _profileService.GetUser(a => a.UserName == name);
            var userViewModel = _mapper.Map<ProfileViewModel>(user);
            return View(userViewModel);

        }
        
        public IActionResult Leaderboard()
        {
            var model = new List<LeaderboardProfileViewModel>();
            var dtos =  _profileService.GetTopFive();
            foreach (var dto in dtos)
            {
                var viewModel = _mapper.Map<LeaderboardProfileViewModel>(dto);
                model.Add(viewModel);               
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Leaderboard(string profileId)
        {           
            return RedirectToAction("Profile", "Profile", new {profileId = profileId});
        }        
    }
}
