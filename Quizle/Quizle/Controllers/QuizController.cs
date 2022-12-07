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

        private readonly IQuizService _quizDataService;
        private readonly IProfileService _userService;
        public QuizController(IQuizService quizDataService, IMapper mapper, IProfileService userService)
        {
            _quizDataService = quizDataService;
            _userService = userService;
        }
        [HttpGet]
        public IActionResult All()
        {
            var listOfQuestions = _quizDataService.GetAllCurrentQuestions();
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

        public IActionResult SelectDifficulty(int? selectedDifficulty)
        {
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
            HttpContext.Session.SetString("CorrectAnswer", quizData.Answers.First(a => a.IsCorrect).Answer.ToString() ?? "");
            HttpContext.Session.SetInt32("Difficulty", (int)selectedDifficulty);
            HttpContext.Session.SetString("Question", quizData.Question);

            var quizViewModel = new QuizViewModel()
            {
                Question = quizData.Question,
                Answers = quizData.Answers,
                CorrectAnswer = quizData.Answers.First(a => a.IsCorrect).Answer,
                Category = quizData.Category,
                Difficulty = (int)Enum.Parse(typeof(Difficulty), quizData.Difficulty),
                Type = quizData.Type
            };
            if (quizViewModel == null)
            {
                return RedirectToAction("All", "Quiz");
            }

            return View(quizViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Quiz(string selectedAnswer)
        {
            var correctAnswer = HttpContext.Session.GetString("CorrectAnswer");
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var question = HttpContext.Session.GetString("Question");
            var difficulty = HttpContext.Session.GetInt32("Difficulty");

            TempData["Flag"] = "FlagExists";

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Quiz", "Quiz");
            }
            if (question == null || difficulty == null)
            {
                return RedirectToAction("Quiz", "Quiz");

            }
            await _userService.UpdateAllUsersHasDoneQuestion(true, a => a.Id == userId);

            await _userService.AddUserQuestion(userId, question, (int)difficulty, selectedAnswer, correctAnswer);


            if (selectedAnswer == correctAnswer && HttpContext.Session.GetInt32("Difficulty") != null)
            {
                await _userService.AwardPoints((int)HttpContext.Session.GetInt32("Difficulty"), userId);
            }
            if (selectedAnswer == "Not Selected")
            {
                return RedirectToAction("Result", "Quiz", new { answeredCorrectly = selectedAnswer == correctAnswer, selected = "You ran out of time!", correct = correctAnswer });
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