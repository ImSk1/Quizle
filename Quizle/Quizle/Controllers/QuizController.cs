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
using System.Security.Claims;

namespace Quizle.Web.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {

        private readonly ILogger<QuizController> _logger;
        private readonly IConfiguration _config;
        private readonly IQuizDataService _quizDataService;
        private readonly IProfileService _userService;
        private readonly IMapper _mapper;
        public QuizController(ILogger<QuizController> logger, IConfiguration configuration, IQuizDataService quizDataService, IMapper mapper, IProfileService userService)
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
            return RedirectToAction("Quiz", "Quiz", new { selectedDifficulty = selectedDifficulty });
        }

        [HttpGet]
        public async Task<IActionResult> Quiz(int? selectedDifficulty)
        {
            if (selectedDifficulty == null)
            {
                return RedirectToAction("All", "Quiz");

            }

            var quizData = await _quizDataService.GetCurrentQuestion(selectedDifficulty);
            HttpContext.Session.SetString("CorrectAnswer", quizData.Answers.Where(a => a.IsCorrect == true).First().Answer.ToString() ?? "");
            HttpContext.Session.SetInt32("Difficulty", (int)selectedDifficulty);
            HttpContext.Session.SetString("Question", quizData.Question);

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
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var question = HttpContext.Session.GetString("Question");
            var difficulty = HttpContext.Session.GetInt32("Difficulty");

            TempData["Flag"] = "FlagExists";

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Quiz");
            }
            if (correctAnswer == null || question == null || difficulty == null)
            {
                return RedirectToAction("Quiz");

            }
            await _userService.UpdateAllUsersHasDoneQuestion(true, a => a.UserName == User.Identity.Name);

            await _userService.AddUserQuestion(userId, question, (int)difficulty, model.SelectedAnswer.Answer, correctAnswer);


            if (model.SelectedAnswer.Answer == correctAnswer && HttpContext.Session.GetInt32("Difficulty") != null)
            {
                await _quizDataService.AwardPoints((int)HttpContext.Session.GetInt32("Difficulty"), User?.Identity?.Name);
            }
            return RedirectToAction("Result", "Quiz", new { answeredCorrectly = model.SelectedAnswer.Answer == correctAnswer, selected = model.SelectedAnswer.Answer, correct = correctAnswer });
        }

        public IActionResult Result(bool answeredCorrectly, string selected, string correct)
        {
            if (TempData["Flag"] == null)
            {
                return RedirectToAction("All", "Quiz");
            }

            var model = new ResultViewModel()
            {
                IsResultCorrect = answeredCorrectly,
                SelectedAnswer = selected,
                CorrectAnswer = correct
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Result()
        {
            return RedirectToAction("All", "Quiz");
        }

    }
}