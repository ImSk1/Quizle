using Quizle.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.UnitTests.Common
{
	public class DataStorage
	{
		public DataStorage()
		{
			SetValues();
		}

		public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
		private ApplicationUser user;
		private ApplicationUser brokeUser;
		//public ApplicationUser UserWithHighestTierBadge { get; set; }
		//public ApplicationUser UserWithRandomBadge { get; set; }
		private void SetValues()
		{
			string id = Guid.NewGuid().ToString();
			user = new ApplicationUser()
			{
				Id = id,
				UserName = "User",
				Email = "user@email.com",
				EmailConfirmed = true,
				CurrentQuizPoints = 50,

			};
			Users.Add(user);
			string id2 = Guid.NewGuid().ToString();
			brokeUser = new ApplicationUser()
			{
				Id = id,
				UserName = "BrokeUser",
				Email = "brokeuser@email.com",
				EmailConfirmed = true,
				CurrentQuizPoints = 0,

			};
			Users.Add(brokeUser);


		}
	}
}
