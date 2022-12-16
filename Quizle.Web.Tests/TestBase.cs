//using Microsoft.AspNetCore.Http;
//using Moq;
//using Quizle.Core.Contracts;
//using Quizle.Core.Models;
//using Quizle.Core.UnitTests.Common;
//using Quizle.DB.Models;
//using Quizle.Web.UnitTests.Mocks;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace Quizle.Web.UnitTests
//{
//	public class TestBase
//	{
//		protected DataStorage dataStorage;
//		protected List<Quiz> quizTable;
//		protected Mock<HttpContext> mockHttpContext;
//		protected Mock<IProfileService> profileServiceMock;

//		[SetUp]
//		public virtual void Setup()
//		{
//			dataStorage = new DataStorage();
//			quizTable = dataStorage.QuizTable;
//			mockHttpContext = new Mock<HttpContext>();

//			string userId = "1";
//			var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
//			{
//				new Claim(ClaimTypes.NameIdentifier, userId)
//			}));
//			mockHttpContext.Setup(s => s.User).Returns(claims);
//			profileServiceMock = new Mock<IProfileService>();

//			Expression<Func<IProfileService, ProfileDto>> expression = a => a.GetUser(It.IsAny<Func<ApplicationUser, bool>>());


//			profileServiceMock.Setup(expression).Returns(dataStorage.UsersDtos.First());

//		}
//	}
//}
