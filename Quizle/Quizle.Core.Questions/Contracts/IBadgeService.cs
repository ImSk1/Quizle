using Quizle.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Contracts
{
    public interface IBadgeService
    {
        List<BadgeDto> GetAllBadges();
        List<string> GetRarities();
        Task AddBadgeAsync(BadgeDto badge);
    }
}
