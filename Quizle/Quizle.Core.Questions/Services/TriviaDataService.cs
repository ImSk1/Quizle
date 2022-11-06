using Newtonsoft.Json;
using Quizle.Core.Questions.Contracts;
using Quizle.Core.Questions.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Questions.Services
{
    public class TriviaDataService : ITriviaDataService
    {
        public async Task<TriviaResponseModel> GetDataAsync(string url)
        {
            var json = await new WebClient().DownloadStringTaskAsync(url);
            if (string.IsNullOrEmpty(json))
            {
                Console.WriteLine("Json is empty");
            }
            var triviaObj = JsonConvert.DeserializeObject<TriviaResponseModel>(json);           
            return triviaObj;
        }
    }
}
