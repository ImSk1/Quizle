using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quizle.Core.Common;
using Quizle.Core.Contracts;
using Quizle.Core.Exceptions;
using Quizle.Core.Models;
using Quizle.DB.Common;
using Quizle.DB.Common.Enums;
using Quizle.DB.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Quizle.Core.Services
{
    public class QuizService : IQuizService
    {
        private readonly IRepository _repository;
        public QuizService(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<QuizDto> GetDataAsync(string? url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("Url not set in configuration.");
            }
            var client = new HttpClient();
            var json = await client.GetStringAsync(url);
            if (string.IsNullOrEmpty(json))
            {
                throw new InvalidApiResponseException();
            }
            var result = JsonConvert.DeserializeObject<QuizResponseModel>(json)?.Results.First();
            if (result == null)
            {
                throw new InvalidApiResponseException();
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

        public async Task AddQuizRange(IEnumerable<QuizDto> quizzes)
        {
            //ADD VALIDATION
            var quizzesDbos = new List<Quiz>();

           
            foreach (var quiz in quizzes)
            {
                Difficulty difficulty;
                bool enumParseSuccess = Enum.TryParse(quiz.Difficulty, out difficulty);
                if (!enumParseSuccess)
                {
                    return;
                }
                var quizDbo = new Quiz()
                {
                    Question = quiz.Question,
                    Category = quiz.Category,
                    Type = quiz.Type,
                    Difficulty = difficulty
                };
                var answers = new List<Answer>();
                foreach (var answer in quiz.Answers)
                {
                    var answerDbo = new Answer()
                    {
                        Text = answer.Answer,
                        IsCorrect = answer.IsCorrect,
                        Quiz = quizDbo
                    };
                    answers.Add(answerDbo);
                }
                quizDbo.Answers = answers;
                quizzesDbos.Add(quizDbo);
            }
            
            

            await _repository.AddRangeAsync<Quiz>(quizzesDbos);
            await _repository.SaveChangesAsync();

        }
        public QuizDto GetCurrentQuestion(int? difficulty)
        {            
            var quizDto = _repository
                .All<Quiz>()
                .Include(a => a.Answers)
                .Where(a => (int)a.Difficulty == difficulty)
                .Select(a => new QuizDto()
                {
                    Question = a.Question,
                    Category = a.Category,
                    Type = a.Type,
                    Difficulty = a.Difficulty.ToString(),
                    Answers = a.Answers.Select(a => new AnswerDto()
                    {
                        Answer = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                })
                .AsEnumerable()
                .Last();
                           
            return quizDto;
        }
        public List<QuizDto> GetAllCurrentQuestions()
        {
            var quizDto = _repository
                .All<Quiz>()
                .Include(a => a.Answers)
                .AsEnumerable()
                .OrderByDescending(a => a.Id)
                .DistinctBy(a => a.Difficulty)
                .Take(3)
                .OrderBy(a => a.Id)
                .Select(a => new QuizDto()
                {
                    Question = a.Question,
                    Category = a.Category,
                    Difficulty = a.Difficulty.ToString(),
                    Type = a.Type,
                    Answers = a.Answers.Select(a => new AnswerDto() 
                    {
                        Answer = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                })                 
                .ToList();                               

            return quizDto;
        }

        


    }
}
