using Quizle.Core.Models;
using Quizle.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Contracts
{
    public interface IProfileService
    {
        Task UpdateAllUsersHasDoneQuestion(bool valueToChangeTo);

        Task UpdateAllUsersHasDoneQuestion(bool valueToChangeTo, Expression<Func<ApplicationUser, bool>> usersToUpdate);
        ProfileDto GetUser(Func<ApplicationUser, bool> predicate);
        Task AddUserQuestion(string userId, string question, int difficulty, string selectedAnswer, string correctAnswer);
        List<ProfileDto> GetTopFive();
        Task AwardPoints(int quizDifficulty, string userId);
    }
}
