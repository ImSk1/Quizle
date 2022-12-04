using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Exceptions;
using Quizle.Core.Models;
using Quizle.DB.Common;
using Quizle.DB.Common.Enums;
using Quizle.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(IRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }
        public async Task UpdateAllUsersHasDoneQuestion(bool valueToChangeTo)
        {
            var users = _repository.All<ApplicationUser>().ToList();
            foreach (var user in users)
            {
                user.HasAnsweredCurrentQuestion = valueToChangeTo;
            }
            _repository.UpdateRange(users);
            await _repository.SaveChangesAsync();
        }
        public async Task UpdateAllUsersHasDoneQuestion(bool valueToChangeTo, Expression<Func<ApplicationUser, bool>> usersToUpdate)
        {
            var users = _repository.All<ApplicationUser>(usersToUpdate).ToList();
            foreach (var user in users)
            {
                user.HasAnsweredCurrentQuestion = valueToChangeTo;
            }
            _repository.UpdateRange(users);
            await _repository.SaveChangesAsync();
        }
        public async Task<ProfileDto> GetCurrentUser(string currentUserId)
        {            
            var user =  await _userManager.Users
                .Include(a => a.ApplicationUsersBadges)
                .ThenInclude(a => a.Badge)
                .Include(a => a.UserQuestions)
                .Where(a => a.Id == currentUserId)
                .Select(a => new ProfileDto()
                {
                    Username = a.UserName,
                    Email = a.Email,
                    CurrentQuestionStatus = a.HasAnsweredCurrentQuestion,
                    Badge = a.ApplicationUsersBadges.OrderByDescending(a => a.Badge.Rarity).Select(a => new BadgeDto
                    {
                        Id = a.Badge.Id,
                        Name = a.Badge.Name,
                        Description = a.Badge.Description,
                        Image = a.Badge.Image,
                        Price = a.Badge.Price,
                        Rarity = a.Badge.Rarity.ToString()
                    }).FirstOrDefault(),

                    AnsweredQuestions = a.UserQuestions.Select(a => new UserQuestionDto()
                    {
                        Id = a.Id,
                        Question = a.Question,
                        Difficulty = a.Difficulty,                        
                        SelectedAnswer = a.SelectedAnswer,
                        CorrectAnswer = a.CorrectAnswer
                    })
                    .ToList()

                })
                .FirstOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException();
            }                        
            
            if (user.AnsweredQuestionsCount != 0)
            {
                var correctAnswers = user.AnsweredQuestions.Where(a => a.SelectedAnswer == a.CorrectAnswer).ToList();
                var questionCount = user.AnsweredQuestions.Count;
                user.AnsweredQuestionsCount = questionCount;
                user.WinratePercent = ((decimal)correctAnswers.Count / (decimal)user.AnsweredQuestionsCount) * 100;
            }
            else
            {
                user.WinratePercent = 0;
            }

            return user;
        }
        public async Task AddUserQuestion(string userId, string question, int difficulty, string selectedAnswer, string correctAnswer)
        {
            string difficultyString = "";
            switch (difficulty)
            {
                case 1: difficultyString = Difficulty.easy.ToString();
                    break;
                case 2: difficultyString = Difficulty.medium.ToString();
                        break;
                case 3: difficultyString = Difficulty.hard.ToString();
                    break;
            }
            var userQuestion = new UserQuestion()
            {
                Question = question,
                Difficulty = difficultyString,
                SelectedAnswer = selectedAnswer,
                CorrectAnswer = correctAnswer,
                UserId = userId
            };
            await _repository.AddAsync<UserQuestion>(userQuestion);
            await _repository.SaveChangesAsync();   
        }

    }
}
