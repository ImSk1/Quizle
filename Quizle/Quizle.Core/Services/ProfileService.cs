using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quizle.Core.Common;
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
using System.Security.Cryptography;
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
            var users = _userManager.Users.ToList();           
            foreach (var user in users)
            {
                user.HasAnsweredCurrentQuestion = valueToChangeTo;
            }
            _repository.UpdateRange(users);
            await _repository.SaveChangesAsync();
        }
        public async Task UpdateAllUsersHasDoneQuestion(bool valueToChangeTo, Expression<Func<ApplicationUser, bool>> usersToUpdate)
        {
            var users = _userManager.Users.Where(usersToUpdate).ToList();
            foreach (var user in users)
            {
                user.HasAnsweredCurrentQuestion = valueToChangeTo;
            }
            _repository.UpdateRange(users);
            await _repository.SaveChangesAsync();
        }
        public ProfileDto GetUser(Func<ApplicationUser, bool> predicate)
        {
           
            var user =  _userManager.Users
				.Include(a => a.ApplicationUsersBadges)
				.ThenInclude(a => a.Badge)
				.Include(a => a.UserQuestions)	
                .AsEnumerable()
                .Where(predicate)
                .Select(a => new ProfileDto()
                {
                    Id = a.Id,
                    Username = a.UserName,
                    Email = a.Email,
                    CurrentQuestionStatus = a.HasAnsweredCurrentQuestion,
                    QuizPoints = a.CurrentQuizPoints,
                    Badge = a.ApplicationUsersBadges.Where(a => a.Badge != null).Select(a => new BadgeDto
                    {
                        Id = a.Badge.Id,
                        Name = a.Badge.Name,
                        Description = a.Badge.Description,
                        Image = a.Badge.Image,
                        Price = a.Badge.Price,
                        Rarity = a.Badge.Rarity.ToString()
                    })
					.OrderByDescending(a => a.Rarity)
                    .FirstOrDefault(),

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
                .FirstOrDefault();
            if (user == null)
            {
                throw new NotFoundException("User not found.");
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
            if (await _userManager.FindByIdAsync(userId) == null)
            {
                throw new NotFoundException();
            }
            if (String.IsNullOrEmpty(question) || String.IsNullOrEmpty(selectedAnswer) || String.IsNullOrEmpty(correctAnswer))
            {
                throw new ArgumentException("Invalid arguments.");
            }
            if (difficulty < 1 || difficulty > 3)
            {
				throw new ArgumentException("Invalid difficulty.");
			}
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
        public  List<ProfileDto> GetTopFive()
        {
            return _userManager
                .Users
                .OrderByDescending(a => a.CurrentQuizPoints)
                .Select(a => new ProfileDto()
                {
                    Id = a.Id,
                    Username = a.UserName,
                    Email = a.Email,
                    CurrentQuestionStatus = a.HasAnsweredCurrentQuestion,
                    QuizPoints = a.CurrentQuizPoints
                })
                .Take(5)
                .ToList();
        }
        public async Task AwardPoints(int quizDifficulty, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException();
            }
            switch (quizDifficulty)
            {
                case 1:
                    user.CurrentQuizPoints += Constants.EasyPointsReward;
                    break;
                case 2:
                    user.CurrentQuizPoints += Constants.MediumPointsReward;
                    break;
                case 3:
                    user.CurrentQuizPoints += Constants.HardPointsReward;
                    break;

                default:
                    throw new ArgumentException("Invalid difficulty");
            }
            _repository.Update(user);
            await _repository.SaveChangesAsync();
        }

    }
    
}
