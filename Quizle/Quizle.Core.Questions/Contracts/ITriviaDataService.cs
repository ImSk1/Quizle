using Quizle.Core.Questions.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Questions.Contracts
{
    public interface ITriviaDataService
    {
        Task<TriviaResponseModel> GetDataAsync(string url);
    }
}
