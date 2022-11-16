using Quizle.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Contracts
{
    public interface IUserService
    {
        Task UpdateAllUsersHasDoneQuestion(bool valueToChangeTo);

        Task UpdateAllUsersHasDoneQuestion(bool valueToChangeTo, Expression<Func<ApplicationUser, bool>> usersToUpdate);
    }
}
