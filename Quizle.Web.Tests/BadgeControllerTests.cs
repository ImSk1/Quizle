using Microsoft.AspNetCore.Mvc;
using Moq;
using Quizle.Core.Contracts;
using Quizle.Core.Services;
using Quizle.Web.Controllers;
using Quizle.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Web.UnitTests
{
	public class BadgeControllerTests : TestBase
	{
		private Mock<IBadgeService> badgeServiceMock;
		private BadgeController badgeController;

		[SetUp]
		public override void Setup()
		{
			base.Setup();
			badgeServiceMock = new Mock<IBadgeService>();
			badgeServiceMock.Setup(a => a.GetAllBadges()).Returns(dataStorage.BadgeDtoTable);
			badgeController = new BadgeController(badgeServiceMock.Object, profileServiceMock.Object);
			badgeController.ControllerContext.HttpContext = mockHttpContext.Object;
		}
		[Test]
		public void BadgeController_All_Returns_Correct_Model()
		{
			var result = badgeController.All() as ViewResult;
			var model = ((List<BadgeViewModel>?)result?.Model) ?? null;
			Assert.IsNotNull(result);
			Assert.IsNotNull(model);
			Assert.IsAssignableFrom<List<BadgeViewModel>>(model);
		}
		[Test]
		public void BadgeController_Buy_Works()
		{
			int badgeId = 1;
			int badgePrice = 10;
			var user = dataStorage.UsersDtos.First();
			user.QuizPoints = 10;
			
			var result = badgeController.Buy(badgeId, badgePrice).Result as RedirectToActionResult;


			Assert.AreEqual("All", result.ActionName);
			badgeServiceMock.Verify(a => a.BuyBadgeAsync(badgeId, user.Id));
			
		}
		[Test]
		public void BadgeController_Buy_With_Less_Qp_Doesnt_Call_BuyBadge()
		{
			int badgeId = 1;
			int badgePrice = 10;
			var user = dataStorage.UsersDtos.First();
			user.QuizPoints = 0;

			var result = badgeController.Buy(badgeId, badgePrice).Result as RedirectToActionResult;


			Assert.AreEqual("All", result.ActionName);
			badgeServiceMock.Verify(a => a.BuyBadgeAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never);

		}

	}
}
