using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Quizle.Core.UnitTests.Common;
using Quizle.Core.UnitTests.Helpers;
using Quizle.DB;
using Quizle.DB.Common;
using Quizle.DB.Models;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace Quizle.Core.UnitTests
{
    public class TestBase
    {       
        protected Mock<UserManager<ApplicationUser>> userManager;
        protected DataStorage dataStorage;
        [SetUp]
        public virtual void Setup()
        {

            dataStorage = new DataStorage();
            this.userManager = MockHelper.MockUserManager(new List<ApplicationUser>(dataStorage.Users));            

        }


    }
}