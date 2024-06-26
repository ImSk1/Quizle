﻿using Quizle.Core.Exceptions;
using Quizle.Core.Models;
using Quizle.DB.Models;
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
        Task BuyBadgeAsync(int badgeId, string userId);
        Task DeleteBadgeAsync(int badgeId);
        Task<bool> ExistsByIdAsync(int id);
        bool UserOwnsBadge(string userId, int badgeId);
        List<UserBadgeDto> GetAllMine(string userId);
        Task SetOnProfileAsync(int badgeId, string userId);
    } 
}
