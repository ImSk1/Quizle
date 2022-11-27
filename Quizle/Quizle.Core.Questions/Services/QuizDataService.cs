using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quizle.Core.Contracts;
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
        public async Task<QuizDto> GetCurrentQuestion(int? difficulty)
        {            
            var count = _repository.Count<Quiz>();
            var quizDto = _repository
                .All<Quiz>()
                .Include(a => a.Answers)
                .Where(a => (int)a.Difficulty == difficulty)
                .ProjectTo<QuizDto>(_mapper.ConfigurationProvider)
                .ToList()
                .Last();
                           
            return quizDto;
        }
        public async Task<List<QuizDto>> GetAllCurrentQuestions()
        {
            var count = _repository.Count<Quiz>();
            var quizDto = _repository
                .All<Quiz>()
                .Include(a => a.Answers)
                .ToList()
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

        public async Task AwardPoints(int quizDifficulty, string username)
        {
            var user = _repository.All<ApplicationUser>().First(a => a.UserName == username);
            switch (quizDifficulty)
            {
                case 1:     
                    user.QuizPoints += 25;                                         
                    break;
                case 2:
                    user.QuizPoints += 50;
                    break;
                case 3:
                    user.QuizPoints += 100;
                    break;                                   
            }
            _repository.Update(user);
            await _repository.SaveChangesAsync();
        }


    }
}
