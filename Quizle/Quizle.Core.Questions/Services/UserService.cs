using Quizle.Core.Contracts;
using Quizle.DB.Common;
using Quizle.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository _repository;

        public UserService(IRepository repository)
        {
            _repository = repository;
        }
        public async Task UpdateAllUsersHasDoneQuestion(bool valueToChangeTo)
        {
            var users = _repository.All<ApplicationUser>().ToList();
            foreach (var user in users)
            {
                user.HasAnsweredCurrentQuestion = valueToChangeTo;
            }
            _repository.UpdateRange(users);
            await _repository.SaveChangesAsync();
        }
        public async Task UpdateAllUsersHasDoneQuestion(bool valueToChangeTo, Expression<Func<ApplicationUser, bool>> usersToUpdate)
        {
            var users = _repository.All<ApplicationUser>(usersToUpdate).ToList();
            foreach (var user in users)
            {
                user.HasAnsweredCurrentQuestion = valueToChangeTo;
            }
            _repository.UpdateRange(users);
            await _repository.SaveChangesAsync();
        }
    }
}
