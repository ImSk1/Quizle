using Quizle.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Quizle.Web.Models.Quiz
{
    public class QuizViewModel
    {
        public string? Question { get; set; }
        public string? Category { get; set; }
        public string? Type { get; set; }
        public int? Difficulty { get; set; }
        public string? CorrectAnswer { get; set; }
        public string? ImageUrl { get; set; }
        public List<AnswerDto>? Answers { get; set; }
        [Required]
        public string SelectedAnswer { get; set; } = null!;

    }
}
