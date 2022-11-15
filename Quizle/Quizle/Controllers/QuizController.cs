using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizle.Core.Questions.Contracts;
using Quizle.Web.Models;
using System.Diagnostics;

namespace Quizle.Web.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        private readonly ILogger<QuizController> _logger;
        private readonly IConfiguration _config;
        private readonly IQuizDataService _quizDataService;
        private readonly IMapper _mapper;

        public QuizController(ILogger<QuizController> logger, IConfiguration configuration, IQuizDataService quizDataService, IMapper mapper)
        {
            _logger = logger;
            _config = configuration;
            _quizDataService = quizDataService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Quiz()
        {
            var quizData = await _quizDataService.GetCurrentQuestion();

            if (quizData == null)
            {
                return View();
            }
            var quizViewModel = _mapper.Map<QuizViewModel>(quizData);
            
            return View(quizViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Quiz(QuizViewModel model)
        {
            var quizData = await _quizDataService.GetCurrentQuestion();


            if (!ModelState.IsValid)
            {
                return RedirectToAction("Quiz");
            }
            if (model.SelectedAnswer == quizData.Answers.Where(a => a.IsCorrect == true).First())
            {
                _logger.LogDebug("Correct!");
                //TODO: Redirect To Correct Answer Page
            }           
           
            return RedirectToAction("Quiz");
        }

    }
}