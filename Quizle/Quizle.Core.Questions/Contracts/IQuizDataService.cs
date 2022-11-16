using Quizle.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Contracts
{
    public interface IQuizDataService
    {
        Task<QuizDto> GetDataAsync(string url);
        Task AddQuizRange(IEnumerable<QuizDto> quizzes);
        Task<QuizDto> GetCurrentQuestion(int difficulty);
    }
}
