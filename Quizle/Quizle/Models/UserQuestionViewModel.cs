namespace Quizle.Web.Models
{
    public class UserQuestionViewModel
    {
        public int Id { get; set; }
        public string Question { get; set; } = null!;
        public string SelectedAnswer { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Difficulty { get; set; } = null!;
        public string CorrectAnswer { get; set; } = null!;
    }
}
