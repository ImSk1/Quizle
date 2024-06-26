﻿using Moq;
using Quizle.Core.Contracts;
using Quizle.Core.Models;
using Quizle.Core.Services;
using Quizle.DB;
using Quizle.DB.Common;
using Quizle.DB.Common.Enums;
using Quizle.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.UnitTests
{
	public class QuizServiceTests : TestBase
	{
		private Mock<IRepository> repositoryMock;
		private IQuizService quizService;
		private List<Quiz> quizTable;
		[SetUp]
		public override void Setup()
		{
			base.Setup();
			quizTable = dataStorage.QuizTable;
			repositoryMock = new Mock<IRepository>();
			repositoryMock.Setup(r => r.All<Quiz>()).Returns(quizTable.AsQueryable());
			repositoryMock.Setup(r => r.GetByIdAsync<Quiz>(It.IsAny<int>()))!.ReturnsAsync((int id) => quizTable.FirstOrDefault(a => a.Id == id));

			quizService = new QuizService(repositoryMock.Object);

		}
		[Test]
		public void QuizService_GetDataAsync_Should_Return_QuizDto()
        {
            //Arrange
            string url = "https://opentdb.com/api.php?amount=1&difficulty=easy";
			//Act
			var model = quizService.GetDataAsync(url);

			Assert.That(model, Is.Not.Null);
			Assert.IsAssignableFrom<QuizDto>(model.Result);
            Assert.Multiple(() =>
            {
                Assert.That(model.Result.Question, Is.Not.Null);
                Assert.That(model.Result.Type, Is.Not.Null);
                Assert.That(model.Result.Difficulty, Is.Not.Null);
                Assert.That(model.Result.Answers, Is.Not.Null);
            });
        }

        [Test]
		public void QuizService_GetDataAsync_With_Null_Url_Throws()
		{
			//Arrange
			string? url = null;

			//Act 
			//Assert
			Assert.ThrowsAsync<ArgumentException>(() => quizService.GetDataAsync(url));
			
		}
		[Test]
		public void QuizService_GetDataAsync_With_Empty_Url_Throws()
		{
			//Arrange
			string? url = "";

			//Act 
			//Assert
			Assert.ThrowsAsync<ArgumentException>(() => quizService.GetDataAsync(url));

		}
		[Test]
		public void QuizService_AddQuizRange_Works()
		{
			//Arrange
			IEnumerable<QuizDto> list = new List<QuizDto>()
			{
				new QuizDto()
				{					
					Type = "Boolean",
					Difficulty = "medium",
					Question = "Question1111111111",
					Category = "Category",
					Answers = new List<AnswerDto>()
				}
			};
			//Act
			quizService.AddQuizRange(list);
			repositoryMock.Verify(a => a.AddRangeAsync<Quiz>(It.IsAny<List<Quiz>>()));
			repositoryMock.Verify(a => a.SaveChangesAsync());
		}
		[Test]
		public void QuizService_AddQuizRange_With_Invalid_Arg_Throws_ArgNullEx()
		{
			//Arrange
			IEnumerable<QuizDto>? list = new List<QuizDto>() 
			{
				new QuizDto()
				{
					Type = "Boolean",
				}
			};
			//Act
			Assert.ThrowsAsync<ArgumentException>(() => quizService.AddQuizRange(list));

		}
		[Test]
		public void GetCurrentQuestion_Returns_Latest_Question_From_Difficulty()
		{
			int difficulty = 1;
			var expected = quizTable.Where(a => (int)a.Difficulty == difficulty).Last().Question;
			var actual = quizService.GetCurrentQuestion(difficulty).Question;

			repositoryMock.Verify(a => a.All<Quiz>());
			Assert.That(expected, Is.EqualTo(actual));
		}
		[Test]
		public void GetAllCurrentQuestions_Returns_Latest_Question_From_Each_Difficulty()
        {
            int difficultyEasy = 1;
			int difficultyMedium = 2;
			int difficultyHard = 3;

			var expectedEasy = quizTable.Where(a => (int)a.Difficulty == difficultyEasy).Last().Question;
			var expectedMedium = quizTable.Where(a => (int)a.Difficulty == difficultyMedium).Last().Question;
			var expectedHard = quizTable.Where(a => (int)a.Difficulty == difficultyHard).Last().Question;

			var model = quizService.GetAllCurrentQuestions();
			Assert.That(model, Is.Not.Null);
			var actualEasy = model?.FirstOrDefault(a => a.Difficulty == "easy")?.Question;
			var actualMedium = model?.FirstOrDefault(a => a.Difficulty == "medium")?.Question;
			var actualHard = model?.FirstOrDefault(a => a.Difficulty == "hard")?.Question;

			repositoryMock.Verify(a => a.All<Quiz>());
            Assert.Multiple(() =>
            {
                Assert.That(expectedEasy, Is.EqualTo(actualEasy));
                Assert.That(expectedMedium, Is.EqualTo(actualMedium));
                Assert.That(expectedHard, Is.EqualTo(actualHard));
            });
        }
    }
}
