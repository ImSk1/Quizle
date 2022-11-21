using Quizle.Core.Models;

namespace Quizle.Web.Models
{
    public class QuizViewModel
    {
        public string? Question { get; set; }
        public string? Category { get; set; }
        public string? Type { get; set; }
        public int? Difficulty { get; set; }
        public string? CorrectAnswer { get; set; }
        public List<AnswerDto>? Answers { get; set; }
        public AnswerDto SelectedAnswer { get; set; }

    }
}
