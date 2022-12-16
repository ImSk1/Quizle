//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;
//using Moq;
//using Quizle.Core.Contracts;
//using Quizle.Core.UnitTests.Common;
//using Quizle.DB.Common.Enums;
//using Quizle.DB.Models;
//using Quizle.Web.Controllers;
//using Quizle.Web.Models;
//using Quizle.Web.UnitTests;
//using Quizle.Web.UnitTests.Mocks;
//using System;
//using System.Linq.Expressions;
//using System.Net.Http;
//using System.Security.Claims;

//namespace Quizle.Web.UnitTests
//{
//	public class QuizControllerTests : TestBase 
//	{
//		private Mock<IQuizService> quizServiceMock;
//		private MockHttpSession mockHttpSession;
//		private QuizController quizController;
//		[SetUp]
//		public override void Setup()
//		{
			
//			base.Setup();
//			quizServiceMock = new Mock<IQuizService>();
//			quizServiceMock.Setup(a => a.GetAllCurrentQuestions()).Returns(dataStorage.CurrentThreeQuestions);
//			quizServiceMock.Setup(a => a.GetCurrentQuestion(It.IsInRange<int>(1,3, Moq.Range.Inclusive))).Returns((int difficulty) => dataStorage.CurrentThreeQuestions.First(a => (int)Enum.Parse(typeof(Difficulty), a.Difficulty) == difficulty));

//			mockHttpSession = new MockHttpSession();
//			mockHttpSession["CorrectAnswer"] = "CorrectAnswer";
//			mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);

//			quizController = new QuizController(quizServiceMock.Object, profileServiceMock.Object);
//			quizController.ControllerContext.HttpContext = mockHttpContext.Object;

			
//		}

//		[Test]
//		public void QuizController_All_Return_Correct_View_With_Model()
//        {
//            var result = quizController.All() as ViewResult;
//			var quiz = ((List<QuizViewModel>?)result?.Model) ?? null;
//			if (quiz == null)
//			{
//				Assert.Fail();
//				return;
//			}
//			Assert.That(quiz, Has.Count.LessThanOrEqualTo(3));
//            Assert.Multiple(() =>
//            {
//                Assert.That(quiz[0].Question, Is.EqualTo(dataStorage.CurrentThreeQuestions[0].Question));
//                Assert.That(quiz[1].Question, Is.EqualTo(dataStorage.CurrentThreeQuestions[1].Question));
//                Assert.That(quiz[2].Question, Is.EqualTo(dataStorage.CurrentThreeQuestions[2].Question));
//            });
//        }

//        [Test]
//		public void QuizController_SelectDifficulty_Redirects_To_Quiz_With_Difficulty()
//        {
//            int difficulty = 1;
//			string action = "Quiz";

//			var result = (RedirectToActionResult)quizController.SelectDifficulty(difficulty);
//            Assert.Multiple(() =>
//            {
//                Assert.That(action, Is.EqualTo(result?.ActionName));
//                Assert.That(difficulty, Is.EqualTo(result?.RouteValues?["selectedDifficulty"] ?? string.Empty));
//            });
//        }
//        [Test]
//		public void QuizController_SelectDifficulty_With_Invalid_Difficulty_Redirects_Back_To_All()
//		{
//			int difficulty = -1;
//			string action = "All";

//			var result = (RedirectToActionResult)quizController.SelectDifficulty(difficulty);
//			Assert.That(action, Is.EqualTo(result?.ActionName));

//		}
//		[Test]
//		public void QuizController_QuizGet_Works()
//		{
//			int selectedDiff = 1;

//			var result = quizController.Quiz(selectedDiff) as ViewResult;
//			Assert.That(result?.Model, Is.Not.Null);
//			Assert.IsAssignableFrom<QuizViewModel>(result.Model);
//		}
//		[Test]
//		public void QuizController_QuizGet_With_Null_Selected_Diff()
//		{
//			int? selectedDiff = null;
//			string action = "All";

//			var result = quizController.Quiz(selectedDiff) as RedirectToActionResult;
//			Assert.That(action, Is.EqualTo(result?.ActionName));
//		}

//		[Test]
//		public void QuizController_QuizPost_Works()
//        {
//            var tempData = new TempDataDictionary(mockHttpContext.Object, Mock.Of<ITempDataProvider>());
//			quizController.TempData = tempData;

//			string expectedCorrect = "CorrectAnswer";
//			string expectedQuestion = "Question";
//			int difficulty = 1;
//			string selecded = It.IsAny<string>();
//			string action = "Result";
				

//			var result = quizController.Quiz(expectedCorrect, expectedQuestion, difficulty).Result as RedirectToActionResult;

//			bool answeredCorrectly;
//			bool answeredCorrectlyParsable = bool.TryParse(result?.RouteValues?["answeredCorrectly"]?.ToString(), out answeredCorrectly);
//			if (answeredCorrectlyParsable == false)
//			{
//				Assert.Fail();
//			}
			
//			string? selected = result?.RouteValues?["selected"]?.ToString();
//			string? correct = result?.RouteValues?["correct"]?.ToString();
//            Assert.Multiple(() =>
//            {
//                Assert.That(action, Is.EqualTo(result?.ActionName));
//                Assert.That(correct, Is.EqualTo(expectedCorrect));
//            });
//			profileServiceMock.Verify(a => a.UpdateAllUsersHasDoneQuestion(It.IsAny<bool>(), It.IsAny<Expression<Func<ApplicationUser, bool>>>()));
//			profileServiceMock.Verify(a => a.AddUserQuestion(It.IsAny<string>(), expectedQuestion, difficulty, selected ?? string.Empty, expectedCorrect));
//			if (selected == expectedCorrect)
//			{
//				profileServiceMock.Verify(a => a.AwardPoints(difficulty, It.IsAny<string>()));
//			}
//        }

//        [Test]
//		public void QuizController_QuizPost_With_Null_Or_Empty_String_Redirects_To_All()
//		{
//			var tempData = new TempDataDictionary(mockHttpContext.Object, Mock.Of<ITempDataProvider>());
//			quizController.TempData = tempData;

//			string expectedCorrect = "CorrectAnswer";
//			string expectedQuestion = "";
//			int difficulty = 1;

//			var result = quizController.Quiz(expectedCorrect, expectedQuestion, difficulty).Result as RedirectToActionResult;
//			Assert.That(result?.ActionName, Is.EqualTo("All"));			

//		}
//		[Test]
//		public void QuizController_QuizPost_Result_Returns_Correct_View_With_Correct_Model()
//		{
//			var tempData = new TempDataDictionary(mockHttpContext.Object, Mock.Of<ITempDataProvider>());
//			tempData["Flag"] = "FlagExists";
//			quizController.TempData = tempData;

//			bool answeredCorrectly = true;
//			string selected = "selected";
//			string correct = "selected";

//			var result = quizController.Result(answeredCorrectly, selected, correct) as ViewResult;

//			Assert.That(result?.Model, Is.Not.Null);
//			Assert.IsAssignableFrom<ViewResult>(result);
//			Assert.IsAssignableFrom<ResultViewModel>(result.Model);
//		}
//		[Test]
//		public void QuizController_QuizPost_Result_With_No_TempData_Flag_Redirects_To_All()
//        {
//            var tempData = new TempDataDictionary(mockHttpContext.Object, Mock.Of<ITempDataProvider>());
//			quizController.TempData = tempData;


//			bool answeredCorrectly = true;
//			string selected = "selected";
//			string correct = "selected";
//			string action = "All";


//            var result = quizController.Result(answeredCorrectly, selected, correct) as RedirectToActionResult;
//            Assert.Multiple(() =>
//            {
//                Assert.That(result, Is.Not.Null);
//                Assert.That(action, Is.EqualTo(result?.ActionName));
//            });
//        }
//    }
//}