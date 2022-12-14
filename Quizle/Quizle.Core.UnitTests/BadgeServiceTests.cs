using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Quizle.Core.Contracts;
using Quizle.Core.Exceptions;
using Quizle.Core.Models;
using Quizle.Core.Services;
using Quizle.Core.UnitTests.Common;
using Quizle.Core.UnitTests.Helpers;
using Quizle.DB.Common;
using Quizle.DB.Common.Enums;
using Quizle.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.UnitTests
{
    public class BadgeServiceTests : TestBase
    {
        private Mock<IRepository> repositoryMock;
        private IBadgeService badgeService;
        private List<Badge> badgeTable;
        [SetUp]
        public void TestInitialize()
        {
            badgeTable = dataStorage.BadgeTable;
            var applicationUserBadges = new List<ApplicationUserBadge>()
            { 
                new ApplicationUserBadge()
                {
                    ApplicationUserId = dataStorage.Users.First().Id, BadgeId = badgeTable.First().Id, IsOnProfile = false,
                },
                new ApplicationUserBadge()
                {
                    ApplicationUserId = dataStorage.Users.First(a => a.UserName == "userWithEverything").Id, BadgeId = badgeTable.First().Id, IsOnProfile = false, Badge = badgeTable.First(), AcquisitionDate = DateTime.Now
                }
                
            };
            repositoryMock = new Mock<IRepository>();
            repositoryMock.Setup(r => r.All<Badge>()).Returns(badgeTable.AsQueryable());
            repositoryMock.Setup(r => r.AllReadonly<Badge>()).Returns(badgeTable.AsQueryable().AsNoTracking());
            repositoryMock.Setup(r => r.GetByIdAsync<Badge>(It.IsAny<int>()))!.ReturnsAsync((int id) => badgeTable.FirstOrDefault(a => a.Id == id));
            repositoryMock.Setup(r => r.AllReadonly<ApplicationUserBadge>()).Returns(applicationUserBadges.AsQueryable);
            repositoryMock.Setup(r => r.All<ApplicationUserBadge>()).Returns(applicationUserBadges.AsQueryable);
            repositoryMock.Setup(r => r.GetByCompositeKey<ApplicationUserBadge>(dataStorage.Users.First().Id, dataStorage.BadgeTable.First().Id))!.ReturnsAsync((object[] ids) => new ApplicationUserBadge() { ApplicationUserId = ids[0].ToString(), BadgeId = (int)ids[1], IsOnProfile = false });
            badgeService = new BadgeService(repositoryMock.Object, this.userManager.Object);

        }
        [Test]
        public void BadgeService_GetAllBadges_Should_Get_All_Badges()
        {
            var actual = badgeService.GetAllBadges();

            //Assert
            repositoryMock.Verify();
            Assert.That(actual, Is.Not.Null);
            Assert.IsAssignableFrom<List<BadgeDto>>(actual);
            Assert.That(actual, Has.Count.EqualTo(badgeTable.Count));

        }

        [Test]
        public void BadgeService_GetRarities_Should_Get_All_Rarities()
        {
            //Arrange
            var expected = new List<string>() { "Common", "Rare", "Epic", "Legendary" };


            //Act
            var actual = badgeService.GetRarities();
            Assert.That(actual, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.IsAssignableFrom<List<string>>(actual);
                Assert.That(expected.Except(actual).Count, Is.EqualTo(0));
            });
        }

        [Test]
        public void BadgeService_AddBadgeAsync_Should_Add_Badge()
        {
            //Arrange

            var badge = new BadgeDto()
            {
                Id = 1,
                Name = "Badge1",
                Description = "Description",
                Rarity = "Rare",
                Price = 5,
                Image = Array.Empty<byte>(),
                OwnerIds = new List<string>()
            };

            //Act
            badgeService.AddBadgeAsync(badge);
            //Assert
            repositoryMock.Verify(v => v.AddAsync<Badge>(It.IsAny<Badge>()), Times.Once);
            repositoryMock.Verify(v => v.SaveChangesAsync(), Times.Once);
        }
        //      [Test]
        //      public void BadgeService_AddBadgeAsync_With_Null_Argument_Should_Throw_ArgumentNullException()
        //      {
        //          //Act
        //          //Assert
        //          Assert.ThrowsAsync<ArgumentNullException>(() => badgeService.AddBadgeAsync(null));
        //}
        [Test]
        public void BadgeService_BuyBadgeAsync_Gives_Badge_To_User()
        {
            //Arrange
            var badge = badgeTable.First();
            int badgeId = badge.Id;
            var user = dataStorage.Users.First();
            string userId = user.Id;
            //Act
            badgeService.BuyBadgeAsync(badgeId, userId);

            //Assert
            this.userManager.Verify(a => a.Users);
            this.repositoryMock.Verify(a => a.GetByIdAsync<Badge>(badgeId), Times.Once);
            this.repositoryMock.Verify(a => a.SaveChangesAsync(), Times.Once);
            Assert.That(user.ApplicationUsersBadges.Any(a => a.ApplicationUserId == userId && a.BadgeId == badgeId));

        }
        [Test]
        public void BadgeService_BuyBadgeAsync_With_Empty_UserId_Throws_ArgumentEx()
        {
            //Arrange
            int badgeId = 1;
            string userId = "";

            //Assert
            Assert.ThrowsAsync<ArgumentException>(() => badgeService.BuyBadgeAsync(badgeId, userId));

        }
        [Test]
        public void BadgeService_BuyBadgeAsync_With_Unavailable_UserId_Throws_NotFoundEx()
        {
            //Arrange
            int badgeId = 1;
            string userId = Guid.NewGuid().ToString();

            //Assert
            Assert.ThrowsAsync<NotFoundException>(() => badgeService.BuyBadgeAsync(badgeId, userId));
        }
        [Test]
        public void BadgeService_BuyBadgeAsync_With_Unavailable_BadgeId_Throws_NotFoundEx()
        {
            //Arrange
            int badgeId = -1;
            string userId = dataStorage.Users.First().Id;

            //Assert
            Assert.ThrowsAsync<NotFoundException>(() => badgeService.BuyBadgeAsync(badgeId, userId));
        }
        [Test]
        public void BadgeService_ExistsByIdAsync_Works()
        {
            int badgeId = dataStorage.BadgeTable.First().Id;
            bool expected = true;

            var actual = badgeService.ExistsByIdAsync(badgeId);
            repositoryMock.Verify(a => a.GetByIdAsync<Badge>(badgeId));
            Assert.That(actual.Result, Is.EqualTo(expected));
        }
        [Test]
        public void BadgeService_DeleteBadgeAsync_Works()
        {
            int badgeId = dataStorage.BadgeTable.First().Id;

            var actual = badgeService.DeleteBadgeAsync(badgeId);
            repositoryMock.Verify(a => a.DeleteAsync<Badge>(badgeId));
            repositoryMock.Verify(a => a.SaveChangesAsync());
        }        
        [Test]
        public void BadgeService_SetOnProfileAsync_With_Invalid_UserId_Throws()
        {
            int badgeId = dataStorage.BadgeTable.First().Id;
            string userId = "";
            Assert.ThrowsAsync<ArgumentException>(() => badgeService.SetOnProfileAsync(badgeId, userId));

        }
        [Test]
        public void BadgeService_UserOwnsBadge_Works()
        {
            int badgeId = dataStorage.BadgeTable.First().Id;
            string userId = dataStorage.Users.First(a => a.UserName == "userWithEverything").Id;
            var actual = badgeService.UserOwnsBadge(userId, badgeId);
            Assert.That(actual, Is.EqualTo(true));

        }
        [Test]
        public void BadgeService_GetAllMine_Works()
        {
            string userId = dataStorage.Users.First(a => a.UserName == "userWithEverything").Id;
            var actual = badgeService.GetAllMine(userId);

            //Assert
            repositoryMock.Verify();
            Assert.That(actual, Is.Not.Null);
            Assert.IsAssignableFrom<List<UserBadgeDto>>(actual);
            Assert.That(actual, Has.Count.EqualTo(1));

        }
    }
}