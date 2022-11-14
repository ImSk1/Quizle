using Newtonsoft.Json;
using Quizle.Core.Questions.Contracts;
using Quizle.Core.Questions.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Quizle.Core.Questions.Services
{
    public class TriviaDataService : ITriviaDataService
    {
        public async Task<QuizDto> GetDataAsync(string url)
        {
            var json = await new WebClient().DownloadStringTaskAsync(url);
            if (string.IsNullOrEmpty(json))
            {
                Console.WriteLine("Json is empty");
            }
            var result = JsonConvert.DeserializeObject<TriviaResponseModel>(json)?.Results.First();
            if (result == null)
            {
                return null;
            }
            var answers = new List<AnswerDto>();
            foreach (var incAnswer in result.IncorrectAnswers)
            {
                answers.Add(new AnswerDto() { Answer = HttpUtility.HtmlDecode(incAnswer), IsCorrect = false });
            }
            answers.Add(new AnswerDto() { Answer = HttpUtility.HtmlDecode(result.CorrectAnswer), IsCorrect = true });
            var quiz = new QuizDto()
            {
                Question = HttpUtility.HtmlDecode(result.Question),
                Difficulty = result.Difficulty,
                Answers = answers,
                Category = result.Category,
                Type = result.Type
            };
            return quiz;
        }
    }
}
