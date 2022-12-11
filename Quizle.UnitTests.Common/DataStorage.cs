using Quizle.Core.Models;
using Quizle.DB.Common.Enums;
using Quizle.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.UnitTests.Common
{
    public class DataStorage
    {
        public DataStorage()
        {
            SetValues();
        }

        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public List<ProfileDto> UsersDtos { get; set; } = new List<ProfileDto>();
        public List<Quiz> QuizTable { get; set; } = new List<Quiz>();
        public List<QuizDto> CurrentThreeQuestions { get; set; } = new List<QuizDto>();
        public List<Badge> BadgeTable { get; set; } = new List<Badge>();
        public List<BadgeDto> BadgeDtoTable { get; set; } = new List<BadgeDto>();



        private void SetValues()
        {
            ApplicationUser? brokeUser;
            ApplicationUser? userThatDidntAnswerQuestion;
            ApplicationUser? userWithEverything;
            ProfileDto? userDto;
            ApplicationUser? user;

            user = new ApplicationUser()
            {
                Id = "1",
                UserName = "User",
                Email = "user@email.com",
                EmailConfirmed = true,
                CurrentQuizPoints = 50,
                HasAnsweredCurrentQuestion = true,

            };
            Users.Add(user);


            string id2 = Guid.NewGuid().ToString();
            brokeUser = new ApplicationUser()
            {
                Id = id2,
                UserName = "BrokeUser",
                Email = "brokeuser@email.com",
                EmailConfirmed = true,
                CurrentQuizPoints = 0,
                HasAnsweredCurrentQuestion = true,

            };
            Users.Add(brokeUser);
            string id3 = Guid.NewGuid().ToString();
            userThatDidntAnswerQuestion = new ApplicationUser()
            {
                Id = id3,
                UserName = "InactiveUser",
                Email = "InactiveUser@email.com",
                EmailConfirmed = true,
                CurrentQuizPoints = 0,
                HasAnsweredCurrentQuestion = false

            };
            Users.Add(userThatDidntAnswerQuestion);

            string id4 = Guid.NewGuid().ToString();
            userWithEverything = new ApplicationUser()
            {
                Id = id4,
                UserName = "userWithEverything",
                Email = "userWithEverything@email.com",
                EmailConfirmed = true,
                CurrentQuizPoints = 0,
                HasAnsweredCurrentQuestion = true,
                UserQuestions = new List<UserQuestion>
                {
                    new UserQuestion()
                    {
                        Id = 5,
                        Question = "Question",
                        CorrectAnswer = "answ",
                        SelectedAnswer = "answ",
                        Difficulty = "easy",
                        UserId = id4,
                    }
                },
                ApplicationUsersBadges = new List<ApplicationUserBadge>
                {
                    new ApplicationUserBadge()
                    {
                        ApplicationUserId = id4,
                        BadgeId = 1,
                        Badge = new Badge()
                        {
                            Id = 1,
                            Name = "Badge",
                            Description = "Badge",
                            Image = Array.Empty<byte>(),
                            Price = 0,
                            Rarity = Rarity.Epic,
                        }
                    }
                }


            };
            Users.Add(userWithEverything);

            QuizTable = new List<Quiz>()
            {
                new Quiz()
                {
                    Id= 1,
                    Type = "Multiple",
                    Difficulty = Difficulty.easy,
                    Question = "Question",
                    Category = "Category",
                    Answers = new List<Answer>()
                },
                new Quiz()
                {
                    Id= 2,
                    Type = "Boolean",
                    Difficulty = Difficulty.medium,
                    Question = "Question",
                    Category = "Category",
                    Answers = new List<Answer>()
                },
                new Quiz()
                {
                    Id= 3,
                    Type = "Boolean",
                    Difficulty = Difficulty.hard,
                    Question = "Question",
                    Category = "Category",
                    Answers = new List<Answer>()
                }
            };
            BadgeTable = new List<Badge>()
            {
                new Badge(){Id = 1, Name = "Badge1", Description = "Description",  Rarity = Rarity.Rare, Price = 5, Image = Array.Empty<byte>()},
                new Badge(){Id = 2, Name = "Badge2", Description = "Description",  Rarity = Rarity.Rare, Price = 5, Image = Array.Empty<byte>()},
                new Badge(){Id = 3, Name = "Badge3", Description = "Description",  Rarity = Rarity.Rare, Price = 5, Image = Array.Empty<byte>()},
                new Badge(){Id = 4, Name = "Badge4", Description = "Description",  Rarity = Rarity.Rare, Price = 5, Image = Array.Empty<byte>() },
            };
            CurrentThreeQuestions = new List<QuizDto>()
            {
                new QuizDto()
                {

                    Type = "Multiple",
                    Difficulty = Difficulty.easy.ToString(),
                    Question = "Question",
                    Category = "Category",
                    Answers = new List<AnswerDto>()
                    {
                        new AnswerDto()
                        {
                            Answer ="aaa",
                            IsCorrect = true
                        }
                    }
                },
                new QuizDto()
                {

                    Type = "Boolean",
                    Difficulty = Difficulty.medium.ToString(),
                    Question = "Question",
                    Category = "Category",
                    Answers = new List<AnswerDto>()
                    {
                        new AnswerDto()
                        {
                            Answer ="aaa",
                            IsCorrect = true
                        }
                    }
                },
                new QuizDto()
                {
                    Type = "Boolean",
                    Difficulty = Difficulty.hard.ToString(),
                    Question = "Question",
                    Category = "Category",
                    Answers = new List<AnswerDto>()
                    {
                        new AnswerDto()
                        {
                            Answer = "aaa",
                            IsCorrect = true
                        }
                    }
                }
            };
            BadgeDtoTable = new List<BadgeDto>()
            {
                new BadgeDto(){Id = 1, Name = "Badge1", Description = "Description",  Rarity = "Rare", Price = 5, Image = Array.Empty<byte>()},
                new BadgeDto(){Id = 2, Name = "Badge2", Description = "Description",  Rarity = "Rare", Price = 5, Image = Array.Empty<byte>()},
                new BadgeDto(){Id = 3, Name = "Badge3", Description = "Description",  Rarity = "Rare", Price = 5, Image = Array.Empty<byte>()},
                new BadgeDto(){Id = 4, Name = "Badge4", Description = "Description",  Rarity = "Rare", Price = 5, Image = Array.Empty<byte>()},
            };
            userDto = new ProfileDto()
            {
                Id = "1",
                Username = "User",
                Email = "user@email.com",
                QuizPoints = 50,
                AnsweredQuestionsCount = 0,
                AnsweredQuestions = new List<UserQuestionDto>(),
                CurrentQuestionStatus = false,
                WinratePercent = 0,
                Badge = BadgeDtoTable[0]

            };
            UsersDtos.Add(userDto);
        }
    }
}
