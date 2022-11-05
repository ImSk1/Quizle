using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Questions.Contracts
{
    public interface IAPIService
    {
        Task<T> GetDataAsync<T>(string url);
    }
}
