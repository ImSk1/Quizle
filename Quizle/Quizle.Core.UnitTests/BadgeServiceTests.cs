using Moq;
using Quizle.Core.Contracts;
using Quizle.Core.Models;
using Quizle.Core.Services;
using Quizle.DB.Common;
using Quizle.DB.Common.Enums;
using Quizle.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.UnitTests
{
    public class BadgeServiceTests
    {
        [Test]
        public void BadgeService_GetAllBadges_Should_Get_All_Badges()
        {
            //Arrange
            var allBadges = new List<Badge>()
            {
                new Badge(){Id = 1, Name = "Badge1", Description = "Description",  Rarity = Rarity.Rare, Price = 5, Image = new byte[0]},
                new Badge(){Id = 2, Name = "Badge2", Description = "Description",  Rarity = Rarity.Rare, Price = 5, Image = new byte[0]},
                new Badge(){Id = 3, Name = "Badge3", Description = "Description",  Rarity = Rarity.Rare, Price = 5, Image = new byte[0]},
                new Badge(){Id = 4, Name = "Badge4", Description = "Description",  Rarity = Rarity.Rare, Price = 5, Image = new byte[0] },
            };
            
            var repositoryMock = new Mock<IRepository>();
            repositoryMock.Setup(r => r.All<Badge>()).Returns(allBadges.AsQueryable());
            IBadgeService service = new BadgeService(repositoryMock.Object);
            //Act
            var actual = service.GetAllBadges();

            //Assert
            repositoryMock.Verify();
            Assert.IsNotNull(actual);
            Assert.IsAssignableFrom<List<BadgeDto>>(actual);
            Assert.That(actual.Count, Is.EqualTo(4));

        }
    }
}
