using Newtonsoft.Json;
using Quizle.Core.Questions.Contracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Questions.Services
{
    public class APIService : IAPIService
    {
        public async Task<T> GetDataAsync<T>(string url)
        {
            var json = await new WebClient().DownloadStringTaskAsync(url);
            if (string.IsNullOrEmpty(json))
            {
                Console.WriteLine("Json is empty");
            }
            var triviaObj = JsonConvert.DeserializeObject<T>(json);           
            return triviaObj;
        }
    }
}
