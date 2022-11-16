using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quizle.Core.Contracts;
using Quizle.Core.Models;
using Quizle.DB.Common;
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
    public class QuizDataService : IQuizDataService
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        public QuizDataService(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;   
        }
        public async Task<QuizDto> GetDataAsync(string url)
        {
            var json = await new WebClient().DownloadStringTaskAsync(url);
            if (string.IsNullOrEmpty(json))
            {
                Console.WriteLine("Json is empty");
            }
            var result = JsonConvert.DeserializeObject<QuizResponseModel>(json)?.Results.First();
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

        public async Task AddQuizRange(IEnumerable<QuizDto> quizzes)
        {
            if (quizzes == null)
            {
                return;
            }
            var quizzesDbos = new List<Quiz>();
            foreach (var quiz in quizzes)
            {
                var quizDbo = new Quiz()
                {
                    Question = quiz.Question,
                    Category = quiz.Category,
                    Type = quiz.Type,
                    Difficulty = quiz.Difficulty
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
        public async Task<QuizDto> GetCurrentQuestion(int difficulty)
        {
            string testDiff = "";
            if (difficulty == 1)
            {
                testDiff = "easy";
            }
            var count = _repository.Count<Quiz>();
            var quizDto = _repository
                .All<Quiz>()
                .Include(a => a.Answers)
                .Where(a => a.Difficulty == testDiff)
                .ProjectTo<QuizDto>(_mapper.ConfigurationProvider)
                .ToList()
                .Last();
                           
            return quizDto;
        }
        //public async Task<AnswerDto> GetCurrentCorrectAnswer()
        //{
        //    var quiz
        //}
    }
}
