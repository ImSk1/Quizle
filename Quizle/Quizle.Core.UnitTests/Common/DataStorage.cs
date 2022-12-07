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
		private ApplicationUser user;
		private ApplicationUser brokeUser;
		private ApplicationUser userThatDidntAnswerQuestion;
		private ApplicationUser userWithEverything;
		//public ApplicationUser UserWithHighestTierBadge { get; set; }
		//public ApplicationUser UserWithRandomBadge { get; set; }
		private void SetValues()
		{
			string id = Guid.NewGuid().ToString();
			user = new ApplicationUser()
			{
				Id = id,
				UserName = "User",
				Email = "user@email.com",
				EmailConfirmed = true,
				CurrentQuizPoints = 50,
				HasAnsweredCurrentQuestion= true,

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
						User = userWithEverything
					}
				},
				ApplicationUsersBadges = new List<ApplicationUserBadge>
				{
					new ApplicationUserBadge()
					{
						ApplicationUserId = id4,
						ApplicationUser = userWithEverything,
						BadgeId = 1,
						Badge = new Badge()
						{
							Id = 1,
							Name = "Badge",
							Description = "Badge",
							Image = new byte[0],
							Price = 0,
							Rarity = Rarity.Epic,							
						}
					}
				}


			};
			Users.Add(userWithEverything);


		}
	}
}
