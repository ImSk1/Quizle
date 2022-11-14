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
        public async Task<QuizDto> GetDataAsync(string url)
        {
            var json = await new WebClient().DownloadStringTaskAsync(url);
            if (string.IsNullOrEmpty(json))
            {
                Console.WriteLine("Json is empty");
            }
            var result = JsonConvert.DeserializeObject<TriviaResponseModel>(json).Results.First();
            var answers = new List<AnswerDto>();
            foreach (var item in result.IncorrectAnswers)
            {
                answers.Add(new AnswerDto() { Answer = item, IsCorrect = false });
            }
            answers.Add(new AnswerDto() { Answer = result.CorrectAnswer, IsCorrect = true });
            var quiz = new QuizDto()
            {
                Question = result.Question,
                Difficulty = result.Difficulty,
                Answers = answers,
                Category = result.Category,
                Type = result.Type
            };
            return quiz;
        }
    }
}
