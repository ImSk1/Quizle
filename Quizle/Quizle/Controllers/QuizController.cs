using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizle.Core.Contracts;
using Quizle.Core.Exceptions;
using Quizle.Core.Models;
using Quizle.DB.Common.Enums;
using Quizle.DB.Models;
using Quizle.Web.Models.Quiz;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Quizle.Web.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {

        private readonly IQuizService _quizDataService;
        private readonly IProfileService _userService;
        private readonly IMapper _mapper;
        public QuizController(IQuizService quizDataService, IProfileService userService, IMapper mapper)
        {
            _quizDataService = quizDataService;
            _userService = userService;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult All()
        {
            var listOfQuestions = _quizDataService.GetAllCurrentQuestions();
            var quizViewModel = _mapper.Map<List<QuizViewModel>>(listOfQuestions);

            return View(quizViewModel);
        }
        [HttpPost]

        public IActionResult SelectDifficulty(int? selectedDifficulty)
        {
            if (selectedDifficulty < 1 || selectedDifficulty > 3)
            {
                return RedirectToAction("All", "Quiz");

            }
            return RedirectToAction("Quiz", "Quiz", new { selectedDifficulty = selectedDifficulty });
        }

        [HttpGet]
        public IActionResult Quiz(int? selectedDifficulty)
        {
            if (selectedDifficulty == null)
            {
                return RedirectToAction("All", "Quiz");

            }

            var quizData = _quizDataService.GetCurrentQuestion(selectedDifficulty);

            TempData["CorrectAnswer"] = quizData.Answers.FirstOrDefault(a => a.IsCorrect)?.Answer.ToString() ?? "";

            var quizViewModel = _mapper.Map<QuizViewModel>(quizData);
            return View(quizViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Quiz(string selectedAnswer, string question, int difficulty)
        {
            if (TempData["CorrectAnswer"] == null)
            {
                return RedirectToAction("All", "Quiz");
            }
            string? correctAnswer = TempData["CorrectAnswer"]?.ToString();
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            TempData["Flag"] = "FlagExists";

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Quiz", "Quiz");
            }
            await _userService.UpdateAllUsersHasDoneQuestion(true, a => a.Id == userId);

            await _userService.AddUserQuestion(userId, question, difficulty, selectedAnswer, correctAnswer);
            if (selectedAnswer == correctAnswer)
            {
                await _userService.AwardPoints(difficulty, userId);
            }
            
            return RedirectToAction("Result", "Quiz", new { answeredCorrectly = selectedAnswer == correctAnswer, selected = selectedAnswer, correct = correctAnswer });
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