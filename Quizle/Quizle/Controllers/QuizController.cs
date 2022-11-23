using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizle.Core.Contracts;
using Quizle.Core.Models;
using Quizle.DB.Common.Enums;
using Quizle.DB.Models;
using Quizle.Web.Models;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Quizle.Web.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        private readonly ILogger<QuizController> _logger;
        private readonly IConfiguration _config;
        private readonly IQuizDataService _quizDataService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private int? _selectedDifficulty;
        public QuizController(ILogger<QuizController> logger, IConfiguration configuration, IQuizDataService quizDataService, IMapper mapper, IUserService userService)
        {
            _logger = logger;
            _config = configuration;
            _quizDataService = quizDataService;
            _mapper = mapper;
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var listOfQuestions = await _quizDataService.GetAllCurrentQuestions();
            var quizViewModel = listOfQuestions
                .Select(a => new QuizViewModel()
                {
                    Question = a.Question,
                    Category = a.Category,
                    Difficulty = (int)Enum.Parse(typeof(Difficulty), a.Difficulty),
                    Type = a.Type,
                    Answers = a.Answers.Select(a => new AnswerDto()
                    {
                        Answer = a.Answer,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                })
                .ToList();            
            return View(quizViewModel);
        }
        [HttpPost]

        public async Task<IActionResult> SelectDifficulty(int? selectedDifficulty)
        {           
            return RedirectToAction("Quiz", "Quiz", new { selectedDifficulty = selectedDifficulty} );          
        }

        [HttpGet]
        public async Task<IActionResult> Quiz(int? selectedDifficulty)
        {
            if (selectedDifficulty == null)
            {
                return RedirectToAction("All", "Quiz");

            }
            _selectedDifficulty = selectedDifficulty;

            var quizData = await _quizDataService.GetCurrentQuestion(selectedDifficulty);
            HttpContext.Session.SetString("CorrectAnswer", quizData.Answers.Where(a => a.IsCorrect == true).First().Answer.ToString() ?? "");            
            if (quizData == null)
            {
                return View();
            }
            var quizViewModel = _mapper.Map<QuizViewModel>(quizData);
            if (quizViewModel == null)
            {
                return RedirectToAction("All", "Quiz");
            }
            
            return View(quizViewModel);
        }
        [HttpPost]        
        public async Task<IActionResult> Quiz(QuizViewModel model)
        {
            var correctAnswer = HttpContext.Session.GetString("CorrectAnswer");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Quiz");
            }
            if (model.SelectedAnswer.Answer == correctAnswer)
            {
                _logger.LogDebug("Correct!");
                await _userService.UpdateAllUsersHasDoneQuestion(true, a => a.UserName == User.Identity.Name);
                //TODO: Redirect To Correct Answer Page
            }           
           
            return RedirectToAction("Quiz");
        }

    }
}