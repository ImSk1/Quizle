using AutoMapper;
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
        private readonly IMapper _mapper;

        public QuizController(ILogger<QuizController> logger, IConfiguration configuration, ITriviaDataService triviaDataService, IMapper mapper)
        {
            _logger = logger;
            _config = configuration;
            _triviaDataService = triviaDataService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Quiz()
        {
            var triviaData = await _triviaDataService.GetDataAsync(_config.GetValue<string>("TriviaUrl"));

            if (triviaData == null)
            {
                return View();
            }
            var quizViewModel = _mapper.Map<QuizViewModel>(triviaData);
            
            return View(quizViewModel);
        }
        [HttpPost]
        public IActionResult Quiz(QuizViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Quiz");
            }
            if (model.SelectedAnswer.IsCorrect == true)
            {
                _logger.LogDebug("Correct!");
                //TODO: Redirect To Correct Answer Page
            }           
           
            return RedirectToAction("Quiz");
        }

    }
}