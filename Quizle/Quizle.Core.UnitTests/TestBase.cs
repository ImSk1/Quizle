//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using Quizle.Core.UnitTests.Helpers;
//using Quizle.DB;
//using Quizle.DB.Common;
//using Quizle.DB.Models;
//using System;
//using static System.Formats.Asn1.AsnWriter;

//namespace Quizle.Core.UnitTests
//{
//    public class TestBase
//    {
//        protected QuizleDbContext context;
//        protected Mock<IRepository> repo;
//        private Mock<IUserStore<string>> userStore;
//        protected Mock<UserManager<ApplicationUser>> userManager;
//        [OneTimeSetUp]
//        public void Setup()
//        {
//            var options = new DbContextOptionsBuilder<QuizleDbContext>()
//                    .UseInMemoryDatabase(databaseName: "QuizleInMemoryDb") 
//                    .Options;
//            this.context = new QuizleDbContext(options);
//            this.context.Add(new ApplicationUser()
//            {
//                Id = Guid.NewGuid().ToString(),
//                UserName = "User1",
//                Email = "user1@email.com",
//            });
//            this.context.SaveChanges();
//            this.repo = new Mock<IRepository>(this.context);
//            this.userStore = new Mock<IUserStore<string>>();
//            this.userManager = MockHelper.MockUserManager();

//        }

        
//    }
//}