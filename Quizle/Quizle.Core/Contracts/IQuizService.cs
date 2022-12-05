using Quizle.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Contracts
{
    public interface IQuizService
    {
        Task<QuizDto> GetDataAsync(string? url);
        Task AddQuizRange(IEnumerable<QuizDto> quizzes);
        QuizDto GetCurrentQuestion(int? difficulty);
        List<QuizDto> GetAllCurrentQuestions();        
    }
}
