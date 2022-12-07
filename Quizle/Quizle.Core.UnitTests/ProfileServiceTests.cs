using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Moq;
using Quizle.Core.Contracts;
using Quizle.Core.Exceptions;
using Quizle.Core.Models;
using Quizle.Core.Services;
using Quizle.Core.UnitTests.Common;
using Quizle.DB.Common;
using Quizle.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.UnitTests
{
    public class ProfileServiceTests : TestBase
    {
        private IProfileService profileService;
		private Mock<IRepository> repositoryMock;


		[SetUp]
        public void TestInitialize()
        {
			repositoryMock = new Mock<IRepository>(); 			
            profileService = new ProfileService(repositoryMock.Object, this.userManager.Object);
		}

        [Test]
        public void ProfileService_UpdateAllUsers_With_No_Predicate_Should_Update_All_Available_Userss_HasDoneQuestion()
        {
            //Arrange
            var allUsers = dataStorage.Users;
            allUsers.ForEach(a => a.HasAnsweredCurrentQuestion = true);

            //Act
            profileService.UpdateAllUsersHasDoneQuestion(false);

            //Assert
            userManager.Verify(a => a.Users);
            repositoryMock.Verify(a => a.UpdateRange(allUsers));
            repositoryMock.Verify(a => a.SaveChangesAsync());
            foreach (var user in allUsers)
            {
                Assert.IsFalse(user.HasAnsweredCurrentQuestion);
            }            

        }
		[Test]
		public void ProfileService_UpdateAllUsers_With_Predicate_Should_Update_All_Available_Userss_HasDoneQuestion()
		{
            string id = dataStorage.Users.First().Id;
            Expression<Func<ApplicationUser, bool>> predicate = (ApplicationUser ap) => ap.Id == id;

			//Arrange
			var usersToUpdate = dataStorage.Users.Where(predicate.Compile()).ToList();
			usersToUpdate.ForEach(a => a.HasAnsweredCurrentQuestion = true);

			//Act
			profileService.UpdateAllUsersHasDoneQuestion(false, predicate);

			//Assert
			userManager.Verify(a => a.Users);
			repositoryMock.Verify(a => a.UpdateRange(usersToUpdate));
			repositoryMock.Verify(a => a.SaveChangesAsync());
			foreach (var user in usersToUpdate)
			{
				Assert.IsFalse(user.HasAnsweredCurrentQuestion);
			}

		}
        [Test]
        public void ProfileService_GetUser_Should_Return_User_With_Info_Matching_The_Arguments_Predicate()
		{
            //Arrange
			var user = dataStorage.Users.First(a => a.UserName == "userWithEverything");
			string expectedId = user.Id;
            //Act
            
            var actual = profileService.GetUser(a => a.Id == expectedId);
            //Assert
			userManager.Verify(a => a.Users);
            Assert.IsNotNull(actual);
            Assert.IsAssignableFrom<List<UserQuestionDto>>(actual.AnsweredQuestions);
            Assert.That(actual.Id, Is.EqualTo(expectedId));
		}
		[Test]
		public void ProfileService_GetUser_With_Invalid_Id_Throws_NotFoundEx()
		{
			//Arrange
			string invalidId = Guid.NewGuid().ToString();
			//Act & Assert

			Assert.Throws<NotFoundException>(() => profileService.GetUser(a => a.Id == invalidId));			
		}
		[Test]
		public void ProfileService_AddUserQuestion_Adds_UserQuestion_To_The_Given_User()
		{
			//Arrange
			var user = dataStorage.Users.First();
			string expectedId = user.Id;
			string question = "question";
			int difficulty = 1;
			string selectedAnswer = "answer";
			string correctAnswer = "answer";
			//Act
			profileService.AddUserQuestion(expectedId, question, difficulty, selectedAnswer, correctAnswer);
			//Assert
			repositoryMock.Verify(a => a.AddAsync<UserQuestion>(It.IsAny<Quizle.DB.Models.UserQuestion>()));
			repositoryMock.Verify(a => a.SaveChangesAsync());			
		}
		[Test]
		public void ProfileService_AddUserQuestion_With_Invalid_UserId_Throws()
		{
			//Arrange
			string expectedId = Guid.NewGuid().ToString();
			string question = "question";
			int difficulty = 1;
			string selectedAnswer = "answer";
			string correctAnswer = "answer";
			//Act
			Assert.ThrowsAsync<NotFoundException>(() => profileService.AddUserQuestion(expectedId, question, difficulty, selectedAnswer, correctAnswer));			
		}
		[Test]
		public void ProfileService_AddUserQuestion_With_Invalid_StringArgument_Throws()
		{
			//Arrange
			var expectedId = dataStorage.Users.First().Id;
			string question = "";
			int difficulty = 1;
			string selectedAnswer = "answer";
			string correctAnswer = "answer";
			//Act
			Assert.ThrowsAsync<ArgumentException>(() => profileService.AddUserQuestion(expectedId, question, difficulty, selectedAnswer, correctAnswer));
		}
		[Test]
		public void ProfileService_AddUserQuestion_With_Invalid_Difficulty_Throws()
		{
			//Arrange
			var expectedId = dataStorage.Users.First().Id;
			string question = "question";
			int difficulty = 10;
			string selectedAnswer = "answer";
			string correctAnswer = "answer";
			//Act
			Assert.ThrowsAsync<ArgumentException>(() => profileService.AddUserQuestion(expectedId, question, difficulty, selectedAnswer, correctAnswer));
		}
		[Test]
		public void ProfileService_GetTopFive_Should_Return_Top_Five_Users_Based_On_QP()
		{
			//Act
			var actual = profileService.GetTopFive();

			//Assert
			userManager.Verify(a => a.Users);
			Assert.IsNotNull(actual);
			Assert.IsAssignableFrom <List<ProfileDto>>(actual);
			Assert.That(actual.Count, Is.LessThanOrEqualTo(5));
			Assert.That(actual, Is.Ordered.Descending.By("QuizPoints"));
		}
		[Test]
		public void ProfileService_AwardPoints_Should_Give_Points_To_User_With_That_Id()
		{
			//Arrange
			int quizDifficultyEasy = 1;
			int quizDifficultyMed = 2;
			int quizDifficultyHard = 3;
			var user = dataStorage.Users.First();
			var id = user.Id;
			int previousQp = user.CurrentQuizPoints;


			//Act
			profileService.AwardPoints(quizDifficultyEasy, id);
			Assert.That(user.CurrentQuizPoints, Is.EqualTo(previousQp + 25));
			userManager.Verify(a => a.FindByIdAsync(id));

			previousQp = user.CurrentQuizPoints;
			profileService.AwardPoints(quizDifficultyMed, id);
			Assert.That(user.CurrentQuizPoints, Is.EqualTo(previousQp + 50));

			previousQp = user.CurrentQuizPoints;
			profileService.AwardPoints(quizDifficultyHard, id);
			Assert.That(user.CurrentQuizPoints, Is.EqualTo(previousQp + 100));


		}
		[Test]
		public void ProfileService_AwardPoints_With_Invalid_User_Id_Throws()
		{
			//Arrange
			int quizDifficulty = 2;
			var id = Guid.NewGuid().ToString();

			//Act
			//Assert
			Assert.ThrowsAsync<NotFoundException>(() => profileService.AwardPoints(quizDifficulty, id));
		}
		[Test]
		public void ProfileService_AwardPoints_With_Invalid_Difficulty_Throws()
		{
			//Arrange
			int quizDifficulty = 10;
			var user = dataStorage.Users.First();
			var id = user.Id;

			//Act
			//Assert
			Assert.ThrowsAsync<ArgumentException>(() => profileService.AwardPoints(quizDifficulty, id));
		}

	}
}
