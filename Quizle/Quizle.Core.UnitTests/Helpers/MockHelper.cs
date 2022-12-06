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
        public static Mock<UserManager<ApplicationUser>> MockUserManager(List<ApplicationUser> ls)
        {

            var store = new Mock<IUserStore<ApplicationUser>>();
            var manager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            manager.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            manager.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

			manager.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

			manager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<ApplicationUser, string>((x, y) => ls.Add(x));

			manager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

			manager.Setup(um => um.FindByNameAsync(
					It.IsAny<string>()))!
				.ReturnsAsync((string username) =>
					ls.FirstOrDefault(u => u.UserName == username));

			manager.Setup(um => um.FindByIdAsync(
				   It.IsAny<string>()))!
			   .ReturnsAsync((string id) =>
				   ls.FirstOrDefault(u => u.Id == id));

            manager.Setup(um => um.Users)
                .Returns(ls.AsQueryable());
			return manager;

        }
    }
}
