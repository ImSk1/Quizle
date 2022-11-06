using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizle.Core.Questions.Contracts;
using Quizle.Models;
using System.Diagnostics;

namespace Quizle.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        private readonly ILogger<QuizController> _logger;
        private readonly IConfiguration _config;
        private readonly ITriviaDataService _triviaDataService;

        public QuizController(ILogger<QuizController> logger, IConfiguration configuration, ITriviaDataService triviaDataService)
        {
            _logger = logger;
            _config = configuration;
            _triviaDataService = triviaDataService;
        }
        [HttpGet]
        public async Task<IActionResult> Quiz()
        {
            var triviaData = await _triviaDataService.GetDataAsync(_config.GetValue<string>("TriviaUrl"));
            var triviaDataResult = triviaData.Results.First();
            var quizViewModel = new QuizViewModel()
            {
                Question = triviaDataResult.Question,
                Answers = new List<string>(triviaDataResult.IncorrectAnswers) { triviaDataResult.CorrectAnswer },
                Type = triviaDataResult.Type,
                Category = triviaDataResult.Category,
                Difficulty = triviaDataResult.Difficulty
            };
            return View(quizViewModel);
        }
        [HttpPost]
        public IActionResult Quiz(QuizViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Quiz");
            }
           
            return RedirectToAction("Quiz");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}