using Microsoft.AspNetCore.Identity;
using Moq;
using Quizle.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.UnitTests.Helpers
{
    public class MockHelper
    {
        public static Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            var manager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            manager.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            manager.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());
            manager.Object.PasswordHasher = new PasswordHasher<ApplicationUser>();
            manager.Object.KeyNormalizer = new UpperInvariantLookupNormalizer();
            return manager;

        }
    }
}
