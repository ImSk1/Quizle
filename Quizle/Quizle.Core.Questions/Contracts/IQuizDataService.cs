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
        Task AddQuiz(QuizDto quiz);
        Task<QuizDto> GetCurrentQuestion();
    }
}
